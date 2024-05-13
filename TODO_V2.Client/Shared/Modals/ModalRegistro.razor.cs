//using BlazorBootstrap;
//using TODO_V2.Client.Pages;
//using TODO_V2.Shared;
//using Microsoft.AspNetCore.Components;
//using Microsoft.AspNetCore.Components.Web;

//namespace TODO_V2.Client.Shared.Modals
//{
//    partial class ModalRegistro
//    {

//        [Parameter]
//        public int Id { get; set; }
//        private string checkPassword { get; set; } = string.Empty;
//        private string UserName { get; set; } = string.Empty;
//        private string Password { get; set; } = string.Empty;
//        private string? TaskName { get; set; } = string.Empty;
//        private string? Email { get; set; } = string.Empty;

//        private bool IsDisabled = true;
//        private string color = "black";

//        List<ToastMessage> messages = new();


//        [Parameter] public EventCallback<MouseEventArgs> Registrar { get; set; }
//        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


//        #region Handlers
//        protected void OnClickRegistro()
//        {
//            //if (CheckFormat(UserName) && CheckFormat(Password))
//            //{
//            //    User user = new(UserName, Password, TaskName, Email, DateTimeOffset.Now.ToString("dd-MM-yyyy"));
//            //    Home.NewUser = user;
//            //    Registrar.InvokeAsync();
//            //}
//        }

//        protected void OnClickClose()
//        {
//            UserName = string.Empty;
//            Password = string.Empty;
//            checkPassword = string.Empty;
//            TaskName = string.Empty;
//            Email = string.Empty;
//            Cerrar.InvokeAsync();
//        }


//        private void ValueChangeHandler()
//        {
//            IsDisabled = string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password) || CheckPasswordHandler();
//        }

//        private bool CheckPasswordHandler()
//        {

//            if (!Password.Equals(checkPassword))
//            {
//                color = "red";
//                return true;
//            }
//            else
//            {
//                color = "black";
//                return false;
//            }

//        }
//        #endregion Handlers

//        private bool CheckFormat(string word)
//        {
//            if (string.IsNullOrWhiteSpace(word) || word.Length < 3)
//            {
//                ShowMessage(ToastType.Warning, "El TaskName de usuario y la contraseña deben tener al menos 3 carácteres");
//                return false;
//            }

//            return true;
//        }

//        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

//        private ToastMessage CreateToastMessage(ToastType toastType, string message)
//        {
//            var toastMessage = new ToastMessage();
//            toastMessage.Type = toastType;
//            toastMessage.Message = message;

//            return toastMessage;
//        }
//    }
//}

