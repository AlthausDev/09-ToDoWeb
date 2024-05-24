using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.IdentityModel.Tokens;
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
        public int? TaskId { get; set; } 


        public string TaskName { get; set; } = string.Empty;
        public string TaskCategory { get; set; }
        public DateTime ExpirationDate { get; set; } = DateTime.Now.Date;
        public string state { get; set; }


        private TaskItem? NewTaskItem;

        private string? DescripcionColor;
        private bool IsInputValid = false;

        List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Crear { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        #region OnClick
        protected async Task OnClickRegistro()
        {       
            if (!IsInputValid)
            {
                ShowMessage(ToastType.Danger, "Introduzca la descripción de su tarea");
                DescripcionColor = ColorsEnum.crimson.ToString();
                return;
            }          
                ClearFields();
                //await Registrar.InvokeAsync();      
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
            IsInputValid = TaskName.IsNullOrEmpty();      
        }              
        
        #endregion Handlers


        #region Api
        private async Task NewItem()
        {            
            await Http.PostAsJsonAsync("TaskItem", NewTaskItem);            
        }

        private async Task EditItem()
        {
            await Http.PutAsJsonAsync("TaskItem", NewTaskItem);
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
        private bool CheckFieldFormat(string fieldValue, string fieldType, ref string fieldColor)
        {
            bool isValid = Validation.CheckFormat(fieldValue, fieldType);
            fieldColor = isValid ? ColorsEnum.lime.ToString() : ColorsEnum.white.ToString();
            return isValid;
        }

        private void ClearFields()
        {
            TaskName = TaskCategory = state = string.Empty;
            PasswordColor = UserNameColor = NameColor = SurnameColor = ClaveColor = ColorsEnum.white.ToString();
        }
        #endregion Aux
    }
}
