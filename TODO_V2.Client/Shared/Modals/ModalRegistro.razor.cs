using BlazorBootstrap;
using TODO_V2.Client.Pages;
using TODO_V2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Client.Shared.Modals
{
    partial class ModalRegistro
    {
        public string Name { get; set; } = string.Empty;
        public string Surname { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string CheckPassword { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;
        private string? UserType { get; set; } = TODO_V2.Shared.Models.Enum.UserType.USUARIO.ToString();


        private bool IsInputValid = false;

        private string PasswordColor = "#fff";
        private string UserNameColor = "#fff";
        private string NameColor = "#fff";
        private string SurnameColor = "#fff";
        private string ClaveColor = "#fff";


        List<ToastMessage> messages = new();


        [Parameter] public EventCallback<MouseEventArgs> Registrar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }

        #region Handlers
        protected async Task OnClickRegistro()
        {
            if (IsInputValid)
            {
                Login.user = new(Name, Surname, UserName, Password, UserType); 
                await Registrar.InvokeAsync();
            }
            else
            {
                ShowMessage(ToastType.Danger, "Los datos introducidos no son correctos");
            }

        }

        protected void OnClickClose()
        {
            UserName = string.Empty;
            Password = string.Empty;
            CheckPassword = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Clave = string.Empty;

            UserType = TODO_V2.Shared.Models.Enum.UserType.USUARIO.ToString();
            Cerrar.InvokeAsync();
        }

        private void ValueChangeHandler()
        {
            IsInputValid = CheckPasswordHandler();
            IsInputValid = CheckUserNameHandler();
            IsInputValid = CheckNameHandler();
            IsInputValid = CheckSurnameHandler();
            CheckClaveHandler();
        }


        private bool CheckPasswordHandler()
        {
            if (Password != string.Empty && CheckPassword != string.Empty)
            {
                if (!Password.Equals(CheckPassword))
                {
                    PasswordColor = Colores.crimson.ToString();
                    return false;
                }
                else
                {
                    PasswordColor = Colores.lime.ToString();
                    return true;
                }
            }
            PasswordColor = Colores.white.ToString();
            return false;

        }

        private bool CheckUserNameHandler()
        {
            if (Validation.CheckFormat(UserName, FieldType.AlphaNumeric.ToString()))
            {
                UserNameColor = Colores.lime.ToString();
                return true;
            }
            else
                UserNameColor = Colores.white.ToString();
            return false;
        }

        private bool CheckNameHandler()
        {
            if (Validation.CheckFormat(Name, FieldType.Alphabetical.ToString()))
            {
                NameColor = Colores.lime.ToString();
                return true;
            }
            else
                NameColor = Colores.white.ToString();
            return false;
        }


        private bool CheckSurnameHandler()
        {
            if (Validation.CheckFormat(Surname, FieldType.Alphabetical.ToString()))
            {
                SurnameColor = Colores.lime.ToString();
                return true;
            }
            else
                SurnameColor = Colores.white.ToString();
            return false;
        }

        private bool CheckClaveHandler()
        {

            if (Validation.CheckKey(Clave))
            {
                ClaveColor = Colores.lime.ToString();
                return true;
            }
            else
            {
                ClaveColor = Colores.white.ToString();
                return false;
            }
        }
        #endregion Handlers

        private void ShowMessage(ToastType toastType, string message) => messages.Add(CreateToastMessage(toastType, message));

        private ToastMessage CreateToastMessage(ToastType toastType, string message)
        {
            var toastMessage = new ToastMessage();
            toastMessage.Type = toastType;
            toastMessage.Message = message;

            return toastMessage;
        }
    }
}
