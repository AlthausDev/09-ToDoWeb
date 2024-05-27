using BlazorBootstrap;
using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Layout;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared.Models;
using ToastType = BlazorBootstrap.ToastType;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
using System.Threading.Tasks;


namespace TODO_V2.Client.Pages
{
    public partial class Todo
    {
        [Parameter]
        public string Id { get; set; }

        private User User { get; set; }

        private Modal ModalInstance = default!;
        private ConfirmDialog dialog = default!;
        [Inject] ToastService ToastService { get; set; } = default!;


        private List<ToastMessage> messages = new();
        private ObservableCollection<TaskItem> TaskItemList { get; set; } = new ObservableCollection<TaskItem>();

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
                    SelectedChangeHandler();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            await GetUserData();
            await GetTaskData();
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


        #region Api
        private async Task GetUserData()
        {
            User = await Http.GetFromJsonAsync<User>($"api/User/{Id}");
        }

        private async Task GetTaskData()
        {
            Debug.WriteLine(Id);
            TaskItemList = new ObservableCollection<TaskItem>(await Http.GetFromJsonAsync<List<TaskItem>>($"api/TaskItem/user/{Id}/tasks"));
            StateHasChanged();
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

        private async Task DeleteTaskAsync()
        {
            var parameters = new Dictionary<string?, object?>();
            parameters.Add("TaskId", 39);
            
            var response = await dialog.ShowAsync<ModalDelete>("Are you sure you want to delete this?", parameters);

            if (response)
            {
                await DeleteTaskItem(39);
                ToastService.Notify(new ToastMessage(ToastType.Success, $"Deleted successfully."));
            }
            else
                ToastService.Notify(new ToastMessage(ToastType.Secondary, $"Delete action canceled."));
        }

        private async Task DeleteTaskItem(int Id)
        {
            await Http.DeleteAsync($"api/TaskItem/{Id}");
            ShowMessage(ToastType.Success, "Tarea eliminada con éxito.");
            await GetTaskData();
            SelectedTaskItem = null;
        }
        #endregion Api

        #region OnClick
        private async Task OnClickLogOut()
        {
            var response = await Http.DeleteAsync("/api/User/logout");
            await storageService.RemoveItemAsync("token");
            await storageService.ClearAsync();
            NavManager.NavigateTo("/login");
            Http.DefaultRequestHeaders.Remove("Authorization");
        }  
        #endregion

        #region SelectRow
        private void SelectTaskItem(TaskItem TaskItem)
        {
            SelectedTaskItem = TaskItem;
            Debug.WriteLine(TaskItem.Name);
            Debug.WriteLine(TaskItem.ToString());
        }

        private string GetRowClass(TaskItem TaskItem)
        {
            return TaskItem == SelectedTaskItem ? "selected-row" : "";
        }
        #endregion SelectRow

        public enum Accion
        {
            Espera,
            Crear,
            Editar
        }

        #region aux
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
        //    TaskItemList.Remove(TaskItem);
        //}
        #endregion

        #region Modal
        private void ShowNewTaskModal()
        {
            AccionActual = Accion.Crear;
            NewTaskItem = new TaskItem();
        }

        private void ShowEditTaskModal(TaskItem taskItem)
        {
            AccionActual = Accion.Editar;
            SelectedTaskItem = taskItem;
        }

        //private async TaskItem execTaskItem()
        //{
        //    if (true)
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
        //    }


        private async Task HideModal()
        {
            SelectedTaskItem = null;
            await ModalInstance.HideAsync();
        }

        #region New Task Item
        private async Task OnClickTaskForm()
        {
            var parameters = new Dictionary<string, object>
             {
                { "UserId", User.Id },
                { "TaskId", SelectedTaskItem?.Id },                
                { "Aceptar", EventCallback.Factory.Create<MouseEventArgs>(this, AddNewTask) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalTask>(title: "Tarea", parameters: parameters);
        }


        private async Task AddNewTask()
        {
            ShowMessage(ToastType.Success, "El Registro se ha realizado exitosamente");
            await HideModal();
        }
        #endregion

        #region Edit
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
        #endregion     

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
        #endregion Modal       

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

        #region Handlers
        private void SelectedChangeHandler()
        {
            IsDisabledEdit = selectedTaskItem == null;
        }

        //private void ValueChangeHandler()
        //{
        //    IsDisabled = (String.IsNullOrWhiteSpace(NewTaskItem.TaskItemName) || String.IsNullOrWhiteSpace(NewTaskItem.State));
        //}       
        #endregion Handlers
    }
}