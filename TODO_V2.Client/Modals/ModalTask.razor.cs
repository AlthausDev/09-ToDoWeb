using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
using System.Net.Http.Json;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Client.Modals
{    
    public partial class ModalTask
    {
        [Parameter]
        public int UserId { get; set; }
        [Parameter]
        public int? TaskId { get; set; }

        public string TaskName { get; set; } = string.Empty;
        public int TaskCategory { get; set; } = 1;
        public DateTime ExpirationDate { get; set; } = DateTime.Now.Date;
        public int StateId { get; set; } = 1;


        private TaskItem? NewTaskItem;

        private string DescripcionColor = "#03e9f4";
        private string ExpirationDateColor = "#03e9f4";

        private bool IsInputValid = false;
        public bool IsEditing { get; private set; } = false;

        private List<ToastMessage> messages = new();

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
            bool isTaskNameValid = CheckTaskNameHandler();
            bool isExpirationDateValid = CheckExpirationDateHandler();

            if (isTaskNameValid && isExpirationDateValid)
            {
                IsInputValid = true;

                if (IsEditing)
                {
                    UpdateTaskItem();
                    await EditItem();
                }
                else
                {
                    CreateNewTaskItem();
                    await NewItem();
                }

                ClearFields();
                await Aceptar.InvokeAsync();
            }
            else
            {
                HandleInvalidInput(isTaskNameValid, isExpirationDateValid);
            }
        }

        private void UpdateTaskItem()
        {
            NewTaskItem.Name = TaskName;
            NewTaskItem.CategoryId = TaskCategory;
            NewTaskItem.StateId = StateId;
            NewTaskItem.ExpirationDate = ExpirationDate;
        }

        private void CreateNewTaskItem()
        {
            NewTaskItem = new TaskItem(TaskCategory, UserId, StateId, TaskName, ExpirationDate);
        }

        private void HandleInvalidInput(bool isTaskNameValid, bool isExpirationDateValid)
        {
            ShowMessage(ToastType.Danger, GetErrorMessage(isTaskNameValid, isExpirationDateValid));
            SetInvalidFieldColors(isTaskNameValid, isExpirationDateValid);
        }

        private string GetErrorMessage(bool isTaskNameValid, bool isExpirationDateValid)
        {
            if (!isTaskNameValid && !isExpirationDateValid)
            {
                return "El nombre de la tarea es inválido y la fecha de expiración es anterior a hoy.";
            }
            else if (!isTaskNameValid)
            {
                return "El nombre de la tarea es inválido. Por favor, introduzca un nombre válido.";
            }
            else // if (!isExpirationDateValid)
            {
                return "La fecha de expiración es anterior a hoy. Por favor, introduzca una fecha válida.";
            }
        }

        private void SetInvalidFieldColors(bool isTaskNameValid, bool isExpirationDateValid)
        {
            DescripcionColor = isTaskNameValid ? string.Empty : ColorsEnum.crimson.ToString();
            ExpirationDateColor = isExpirationDateValid ? string.Empty : ColorsEnum.crimson.ToString();
        }


        protected void OnClickClose()
        {
            ClearFields();
            _ = Cerrar.InvokeAsync();
        }

        #endregion OnClick

        #region Handlers
        private void ValueChangeHandler()
        {
            bool isTaskNameValid = CheckTaskNameHandler();
            bool isExpirationDate = CheckExpirationDateHandler();

            IsInputValid = isTaskNameValid && isExpirationDate;
        }

        private bool CheckTaskNameHandler()
        {
            return CheckFieldFormat(TaskName, FieldTypeEnum.AlphaNumeric.ToString(), ref DescripcionColor);
        }

        private bool CheckExpirationDateHandler()
        {
            ExpirationDateColor = "#03e9f4";
            return ExpirationDate.Date >= DateTime.Now.Date;
        }

        private bool CheckFieldFormat(string fieldValue, string fieldType, ref string fieldColor)
        {
            bool isValid = Validation.CheckFormat(fieldValue, fieldType);            
            
            fieldColor = isValid ? ColorsEnum.lime.ToString() : "#03e9f4";
            return isValid;
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
            _ = await Http.PostAsJsonAsync("api/TaskItem", NewTaskItem);
        }

        private async Task EditItem()
        {
            _ = await Http.PutAsJsonAsync($"api/TaskItem/{TaskId}", NewTaskItem);
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
