﻿using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Data;
using TODO_V2.Client.Modals;
using TODO_V2.Shared.Models;
using ToastType = BlazorBootstrap.ToastType;


namespace TODO_V2.Client.Components
{
    //TODO Revisar PreloadService, no se muestra cuando se realiza recarga forzada de la página "F5"
    public partial class TaskList
    {
        /// <summary>
        /// Parámetro que recibe el ID del usuario.
        /// </summary>
        [Parameter]
        public string Id { get; set; }

        /// <summary>
        /// El usuario actual.
        /// </summary>
        private User User { get; set; }

        /// <summary>
        /// Instancia del componente Modal para mostrar diálogos modales.
        /// </summary>
        private Modal ModalInstance = default!;

        /// <summary>
        /// Instancia del componente ConfirmDialog para mostrar cuadros de diálogo de confirmación.
        /// </summary>
        private ConfirmDialog dialog = default!;

        /// <summary>
        /// Servicio para mostrar notificaciones tipo Toast.
        /// </summary>
        [Inject] private ToastService ToastService { get; set; } = default!;

        /// <summary>
        /// Servicio para mostrar indicadores de precarga.
        /// </summary>
        [Inject] protected PreloadService PreloadService { get; set; }

        /// <summary>
        /// Lista de mensajes Toast.
        /// </summary>
        private List<ToastMessage> messages = new();

        /// <summary>
        /// Componente Grid para mostrar la lista de tareas.
        /// </summary>
        private Grid<TaskItem> DataGrid = default!;

        /// <summary>
        /// Colección observable de tareas del usuario.
        /// </summary>
        private ObservableCollection<TaskItem> TaskItemList { get; set; } = new ObservableCollection<TaskItem>();

        /// <summary>
        /// La tarea seleccionada actualmente.
        /// </summary>
        private TaskItem? selectedTaskItem { get; set; } = null;

        /// <summary>
        /// Propiedad para obtener y establecer la tarea seleccionada.
        /// </summary>
        public TaskItem? SelectedTaskItem
        {
            get
            {
                return selectedTaskItem;
            }
            set
            {
                if (selectedTaskItem != value)
                {
                    selectedTaskItem = value;
                }
            }
        }

        /// <summary>
        /// Indicador de si la página está cargando.
        /// </summary>
        private bool isLoading = true;

        /// <summary>
        /// Método que se ejecuta al inicializar el componente.
        /// Carga los datos del usuario y sus tareas.
        /// </summary>
        protected override async Task OnInitializedAsync()
        {
            try
            {
                isLoading = true;
                PreloadService.Show(SpinnerColor.Light, "Cargando...");

                await CategoryDictionary.LoadCategoryDictionary(Http);
                await GetUserData();
                await GetTaskData();
            }
            finally
            {
                isLoading = false;
                await Task.Delay(1000);
                PreloadService.Hide();
            }
        }

        private RenderFragment RenderLoadingIndicator() => builder =>
        {
            if (isLoading)
            {
                builder.OpenElement(0, "div");
                builder.AddAttribute(1, "class", "loading-indicator");
                builder.AddContent(2, "Cargando...");
                builder.CloseElement();
            }
        };


        #region StartUp
        private async Task GetUserData()
        {
            User = await Http.GetFromJsonAsync<User>($"api/User/{Id}");
        }

        private async Task<GridDataProviderResult<TaskItem>> UsersDataProvider(GridDataProviderRequest<TaskItem> request)
        {
            Stopwatch stopwatch = new();
            stopwatch.Start();

            while (TaskItemList.IsNullOrEmpty())
            {
                await Task.Delay(5);
            }

            stopwatch.Stop();
            Debug.WriteLine($"Tiempo total de espera: {stopwatch.ElapsedMilliseconds} ms");

            return await Task.FromResult(request.ApplyTo(TaskItemList.OrderBy(TaskItem => TaskItem.Id)));
        }


        private async Task GetTaskData()
        {
            TaskItemList = new ObservableCollection<TaskItem>(await Http.GetFromJsonAsync<List<TaskItem>>($"api/TaskItem/user/{Id}/tasks"));
        }
        #endregion

        #region SelectRow    
        private async Task SelectTaskItem(GridRowEventArgs<TaskItem> args)
        {
            SelectedTaskItem = args.Item;
        }

        #endregion SelectRow        

        #region Modal  
        private async Task HideModal()
        {
            SelectedTaskItem = null;
            await ModalInstance.HideAsync();
        }

        #region Task Item Form    
        private async Task OnClickTaskForm(TaskItem? taskItem)
        {
            SelectedTaskItem = taskItem;
            var parameters = new Dictionary<string, object>
             {
                { "UserId", User.Id },
                { "TaskId", SelectedTaskItem?.Id },
                { "Aceptar", EventCallback.Factory.Create<MouseEventArgs>(this, TaskFormResult) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalTask>(title: "Tarea", parameters: parameters);
        }


        private async Task TaskFormResult()
        {
            ShowMessage(ToastType.Success, "El Registro se ha realizado exitosamente");
            await HideModal();
            await GetTaskData();
            await DataGrid.RefreshDataAsync();
        }
        #endregion

        #endregion Modal       

        #region Delete

        private async Task DeleteTaskAsync(TaskItem taskItem)
        {
            SelectedTaskItem = taskItem;
            if (SelectedTaskItem == null)
            {
                ToastService.Notify(new ToastMessage(ToastType.Warning, "Ninguna tarea seleccionada."));
                return;
            }

            var parameters = new Dictionary<string, object?>
            {
                { "Name", SelectedTaskItem.Name },
                { "Message", "esta tarea" }
            };

            var options = new ConfirmDialogOptions
            {
                YesButtonColor = ButtonColor.Danger,
                YesButtonText = "Eliminar",
                NoButtonText = "Cancelar",
                IsVerticallyCentered = true,
                Dismissable = true
            };

            var response = await dialog.ShowAsync<ModalDelete>(
                title: "Confirmar Eliminación",
                parameters,
                confirmDialogOptions: options);

            if (response)
            {
                await DeleteTaskItem(SelectedTaskItem.Id);
            }
            else
            {
                ToastService.Notify(new ToastMessage(ToastType.Secondary, "Acción de eliminación cancelada."));
            }
        }


        private async Task DeleteTaskItem(int Id)
        {
            SelectedTaskItem = null;

            _ = await Http.DeleteAsync($"api/TaskItem/{Id}");
            await GetTaskData();
            await DataGrid.RefreshDataAsync();

            ShowMessage(ToastType.Success, "Tarea eliminada con éxito.");
        }
        #endregion 

        #region LogOut
        private async Task OnClickLogOut()
        {
            _ = await Http.DeleteAsync("/api/User/logout");
            await storageService.RemoveItemAsync("token");
            await storageService.ClearAsync();
            NavManager.NavigateTo("/login");
            _ = Http.DefaultRequestHeaders.Remove("Authorization");
        }
        #endregion

        #region Toast
        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        {
            var toastMessage = new ToastMessage();
            toastMessage.Type = toastType;
            toastMessage.Message = message;

            return toastMessage;
        }
        #endregion Toast    

    }
}