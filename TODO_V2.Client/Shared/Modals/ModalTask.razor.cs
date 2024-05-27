using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Diagnostics;
using System.Net.Http.Json;
using TODO_V2.Client.Pages;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Client.Shared.Modals
{
    public partial class ModalTask
    {
        [Parameter]
        public int UserId {  get; set; }
        [Parameter]
        public int? TaskId { get; set; }

        public string TaskName { get; set; } = string.Empty;    
        public int TaskCategory { get; set; } = 1;
        public DateTime ExpirationDate { get; set; } = DateTime.Now.Date;
        public int StateId { get; set; } = 1;


        private TaskItem? NewTaskItem;

        private string DescripcionColor = "#03e9f4";

        private bool IsInputValid = false;
        public bool IsEditing { get; private set; } = false;


        List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Aceptar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            if (TaskId.HasValue)
            {
                IsEditing = true;
                IsInputValid = true;
                await LoadTaskById(TaskId.Value);
            }
            else
            {
                IsEditing = false;
                IsInputValid = false;
                ClearFields();
            }

            await base.OnParametersSetAsync();
        }


        #region OnClick     
        protected async Task OnClickAceptar()
        {
            if (IsInputValid)
            {               
                if (IsEditing)
                {                    
                    NewTaskItem.Name = TaskName;
                    NewTaskItem.CategoryId = TaskCategory;
                    NewTaskItem.StateId = StateId;
                    NewTaskItem.ExpirationDate = ExpirationDate;

                    await EditItem();
                }
                else
                {
                    NewTaskItem = new(TaskCategory, UserId, StateId, TaskName, ExpirationDate);

                    await NewItem();
                }

                Debug.WriteLine($"Nombre de la tarea: {NewTaskItem.Name}");
                Debug.WriteLine($"Categoría de la tarea: {NewTaskItem.CategoryId}");
                Debug.WriteLine($"Fecha de expiración: {NewTaskItem.ExpirationDate}");
                Debug.WriteLine($"Estado de la tarea: {NewTaskItem.StateId}");

                ClearFields();            
                await Aceptar.InvokeAsync();
            }
            else {
                ShowMessage(ToastType.Danger, "Introduzca la descripción de su tarea");
                DescripcionColor = ColorsEnum.crimson.ToString();
                return;
            }
        }

        protected void OnClickClose()
        {
            ClearFields();
            Cerrar.InvokeAsync();
        }

        #endregion OnClick

        #region Handlers
        private void ValueChangeHandler()
        {
            DescripcionColor = "#03e9f4";
            IsInputValid = !TaskName.IsNullOrEmpty();
        }

        #endregion Handlers

        #region Api
        private async Task LoadTaskById(int taskId)
        {
            if (taskId != 0) 
            {
                try
                {
                    var task = await Http.GetFromJsonAsync<TaskItem>($"api/TaskItem/{taskId}");

                    if (task != null)
                    {
                        NewTaskItem = task;
                        
                        TaskName = NewTaskItem.Name;
                        TaskCategory = NewTaskItem.CategoryId;
                        ExpirationDate = (DateTime)NewTaskItem.ExpirationDate;
                        StateId = NewTaskItem.StateId;
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la tarea por Id: {ex.Message}");                  
                }
            }
            else
            {               
                NewTaskItem = new TaskItem();
            }
        }

        private async Task NewItem()
        {
            await Http.PostAsJsonAsync("api/TaskItem", NewTaskItem);
        }

        private async Task EditItem()
        {
            await Http.PutAsJsonAsync($"api/TaskItem/{TaskId}", NewTaskItem);
        }
        #endregion Api

        #region Toast
        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        {
            ToastMessage toastMessage = new();
            toastMessage.Type = toastType;
            toastMessage.Message = message;

            return toastMessage;
        }
        #endregion Toast

        #region Aux
        private void ClearFields()
        {
            TaskName = string.Empty;
            TaskCategory = StateId = 1;
            IsInputValid = false;
            DescripcionColor = "#03e9f4";
        }
        #endregion Aux
    }
}
