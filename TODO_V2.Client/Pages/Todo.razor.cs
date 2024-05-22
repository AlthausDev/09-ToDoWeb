using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Layout;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared.Models;
using ToastType = BlazorBootstrap.ToastType;

namespace TODO_V2.Client.Pages
{
    public partial class Todo
    {
        [Parameter]
        public string Id { get; set; }

        private User User { get; set; }

        private Modal ModalInstance = default!;


        private List<ToastMessage> messages = new();
        private List<TaskItem> TaskItemsList { get; set; } = new List<TaskItem>();

        private bool IsDisabled { get; set; } = true;
        private bool IsDisabledEdit { get; set; } = true;

        private Accion AccionActual { get; set; } = Accion.Espera;


        private TaskItem newTaskItem { get; set; } = new TaskItem();
        private TaskItem NewTaskItem
        {
            get
            {
                return newTaskItem;
            }
            set
            {
                if (newTaskItem != value)
                {
                    newTaskItem = value;
                }
            }
        }

        private TaskItem? selectedTaskItem { get; set; } = null;
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
                    //SelectedChangeHandler();
                }
            }
        }


        protected override async Task OnInitializedAsync()
        {
            await GetUserData();
            await GetTaskData();
        }

        private async Task GetUserData()
        {
            User = await Http.GetFromJsonAsync<User>($"api/User/{Id}");
        }

        private async Task GetTaskData()
        {
            Debug.WriteLine(Id);
            TaskItemsList = await Http.GetFromJsonAsync<List<TaskItem>>($"api/TaskItem/user/{Id}/tasks");
        }

        private async Task AddOrUpdateTaskItem()
        {
            if (AccionActual == Accion.Crear)
            {
                await Http.PostAsJsonAsync("api/TaskItem", NewTaskItem);
                ShowMessage(ToastType.Success, "Tarea creada con éxito.");
            }
            else if (AccionActual == Accion.Editar)
            {
                await Http.PutAsJsonAsync($"api/TaskItem/{SelectedTaskItem.Id}", SelectedTaskItem);
                ShowMessage(ToastType.Success, "Tarea actualizada con éxito.");
            }

            await GetTaskData();
            await HideModal();
        }

        private async Task DeleteTaskItem()
        {
            await Http.DeleteAsync($"api/TaskItem/{SelectedTaskItem.Id}");
            ShowMessage(ToastType.Success, "Tarea eliminada con éxito.");
            await GetTaskData();
            SelectedTaskItem = null;
        }

        #region OnClick
        private async Task OnClickLogOut()
        {
            var response = await Http.DeleteAsync("/api/User/logout");
            await storageService.RemoveItemAsync("token");
            await storageService.ClearAsync();
            NavManager.NavigateTo("/login");
            Http.DefaultRequestHeaders.Remove("Authorization");
        }

        private async Task OnClickNewTask()
        {
            var response = await Http.DeleteAsync("/api/User/logout");
            await storageService.RemoveItemAsync("token");
            await storageService.ClearAsync();
            NavManager.NavigateTo("/login");
            Http.DefaultRequestHeaders.Remove("Authorization");
        }

        #endregion

        #region New Task Item
        private async Task OnClickRegister()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Registrar", EventCallback.Factory.Create<MouseEventArgs>(this, Registro) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalRegistro>(title: "Registrarse", parameters: parameters);
        }


        private async Task Registro()
        {
            ShowMessage(ToastType.Success, "El Registro se ha realizado exitosamente");
            await HideModal();
        }

        private async Task HideModal()
        {
           
            await ModalInstance.HideAsync();
        }
        #endregion 

        //#region Edit
        //private async Task OnClickRegister()
        //{
        //    var parameters = new Dictionary<string, object>
        //    {
        //        { "Registrar", EventCallback.Factory.Create<MouseEventArgs>(this, Registro) },
        //        { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
        //    };
        //    await ModalInstance.ShowAsync<ModalRegistro>(title: "Registrarse", parameters: parameters);
        //}


        //private async Task Registro()
        //{
        //    ShowMessage(ToastType.Success, "El Registro se ha realizado exitosamente");
        //    await HideModal();
        //}

        //private async Task HideModal()
        //{
        //    user = new User();
        //    await ModalInstance.HideAsync();
        //}
        //#endregion 

        private void ShowNewTaskModal()
        {
            AccionActual = Accion.Crear;
            NewTaskItem = new TaskItem();
            //ShowModal();
        }

        private void ShowEditTaskModal(TaskItem taskItem)
        {
            AccionActual = Accion.Editar;
            SelectedTaskItem = taskItem;
            //ShowModal();
        }

        //private async Task HideModal()
        //{
        //    SelectedTaskItem = null;
        //    await ModalInstance.HideAsync();
        //}

        //private void ShowModal()
        //{
        //    ModalInstance.Show();
        //}

        private void ShowMessage(ToastType toastType, string message)
        {
            // Lógica para mostrar un mensaje de toast.
        }

        public enum Accion
        {
            Espera,
            Crear,
            Editar
        }









        //#region ApiOperations    
        //private async TaskItem getData()
        //{
        //    TaskItem[]? taskitemsArray = await Http.GetFromJsonAsync<TaskItem[]>("TaskItem");

        //    if (taskitemsArray is not null)
        //    {
        //        TaskItemsList = [.. taskitemsArray];
        //    }
        //}


        //private async TaskItem Post()
        //{
        //    TaskItems.Add(newTaskItem);
        //    await Http.PostAsJsonAsync("TaskItem", NewTaskItem);
        //    await getData();
        //}

        //private async TaskItem Put()
        //{
        //    await Http.PutAsJsonAsync("TaskItem", NewTaskItem);

        //    TaskItems.Insert(TaskItems.IndexOf(selectedTaskItem), newTaskItem);
        //    TaskItems.Remove(selectedTaskItem);
        //}

        //private async TaskItem Delete()
        //{
        //    if (selectedTaskItem != null)
        //    {
        //        TaskItems.Remove(selectedTaskItem);
        //        HttpResponseMessage httpResponseMessage = await Http.DeleteAsync($"/DelTaskItem/{selectedTaskItem.Id}");

        //        await getData();
        //        SelectedTaskItem = null;

        //        if (accion.Equals(Accion.Espera))
        //        {
        //            ShowMessage(ToastType.Danger, "Registro eliminado con éxito");
        //        }
        //    }
        //}
        //#endregion ApiOperations
        //#region aux

        //private void ShowNewTaskItemModal()
        //{
        //    //taskitemFormRef.SetTaskItem(new TaskItem { CreationDate = DateTime.Now });
        //    //modalRef.Show();
        //}

        //private void ShowEditTaskItemModal(TaskItem TaskItem)
        //{
        //    //taskitemFormRef.SetTaskItem(TaskItem);
        //    //modalRef.Show();
        //}

        //private void SaveTaskItem()
        //{
        //    //var TaskItem = taskitemFormRef.GetTaskItem();
        //    //if (!TaskItems.Contains(TaskItem))
        //    //{
        //    //    TaskItems.Add(TaskItem);
        //    //}
        //    //modalRef.Hide();
        //}

        //private void DeleteTaskItem(TaskItem TaskItem)
        //{
        //    TaskItemsList.Remove(TaskItem);
        //}

        //#endregion



        //#region Modal
        //private async TaskItem execTaskItem()
        //{
        //    if (accion.Equals(Accion.Crear))
        //    {
        //        await Post();
        //        ShowMessage(ToastType.Success, "Registro agregado con éxito");
        //    }
        //    else
        //    {
        //        await Put();
        //        ShowMessage(ToastType.Warning, "Registro editado con éxito");
        //    }
        //    await HideModal();
        //}


        //private async TaskItem OnClickShowModal(Enum accion)
        //{
        //    this.accion = accion;

        //    if (accion.Equals(Accion.Editar))
        //    {
        //        NewTaskItem = selectedTaskItem;
        //    }

        //    if (accion.Equals(Accion.Crear))
        //    {
        //        NewTaskItem = new TaskItem();
        //    }

        //    await modal.ShowAsync();
        //}

        //private async TaskItem HideModal()
        //{
        //    accion = Accion.Espera;
        //    SelectedTaskItem = null;
        //    NewTaskItem = new TaskItem();
        //    await modal.HideAsync();
        //}
        //#endregion Modal       

        //#region SelectRow
        //private void selectTaskItem(TaskItem TaskItem)
        //{
        //    SelectedTaskItem = TaskItem;
        //    //Console.WriteLine(TaskItem.Name);
        //}

        //private string GetRowClass(TaskItem TaskItem)
        //{
        //    return TaskItem == SelectedTaskItem ? "selected-row" : "";
        //}
        //#endregion SelectRow

        //#region AutoComplete
        //private async TaskItem<AutoCompleteDataProviderResult<TaskItem>> TaskItemsDataProvider(AutoCompleteDataProviderRequest<TaskItem> request)
        //{
        //    return await TaskItem.FromResult(request.ApplyTo(TaskItems.OrderBy(TaskItem => TaskItem.TaskItemName)));
        //}

        //#endregion AutoComplete

        //#region Toast
        //private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        //private ToastMessage CreateToastMessage(ToastType toastType, string message)
        //{
        //    var toastMessage = new ToastMessage();
        //    toastMessage.Type = toastType;
        //    toastMessage.Message = message;

        //    return toastMessage;
        //}
        //#endregion Toast     

        //#region Handlers
        //private void SelectedChangeHandler()
        //{
        //    IsDisabledEdit = selectedTaskItem == null;
        //}

        //private void ValueChangeHandler()
        //{
        //    IsDisabled = (String.IsNullOrWhiteSpace(NewTaskItem.TaskItemName) || String.IsNullOrWhiteSpace(NewTaskItem.State));
        //}

        //private async TaskItem OnAutoCompleteChanged(TaskItem TaskItem)
        //{
        //    SelectedTaskItem = TaskItem;
        //    //await JS.InvokeVoidAsync($"searchFuction('{SelectedTaskItem.TaskItemName}')");
        //    Console.WriteLine($"'{TaskItem?.TaskItemName}' selected.");
        //}
        //#endregion Handlers

        //#region Enums
        //public enum Accion
        //{
        //    Espera,
        //    Crear,
        //    Editar
        //}

        //public enum IsFinalizado
        //{
        //    No,
        //    Si
        //}
        //#endregion Enums     
    }

}
