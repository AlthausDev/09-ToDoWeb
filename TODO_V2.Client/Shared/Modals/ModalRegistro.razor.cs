using BlazorBootstrap;
using TODO_V2.Client.Pages;
using TODO_V2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TODO_V2.Shared.Models;

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
        private string? UserType { get; set; } = "User";

        User user = new();

        private bool IsDisabled = true;

        private string PasswordColor = "#fff";     
        private string UserNameColor = "#fff";        
        private string NameColor = "#fff";
        private string SurnameColor = "#fff";
        private string ClaveColor = "#fff";        
        

        List<ToastMessage> messages = new();


        [Parameter] public EventCallback<MouseEventArgs> Registrar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }

        #region Handlers
        protected void OnClickRegistro()
        {
            if (CheckFormat(UserName) && CheckFormat(Password))
            {
                user = new(Name, Surname, UserName, Password, UserType);
                Registrar.InvokeAsync();
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

            //UserType = TODO_V2.Shared.UserType.USUARIO.ToString();
            UserType = "user";
            Cerrar.InvokeAsync();
        }

        private void ValueChangeHandler()
        {
            CheckPasswordHandler();
            CheckUserNameHandler();
            CheckNameHandler();
            CheckSurnameHandler();
            CheckClaveHandler();


            //IsDisabled = CheckPasswordHandler() || string.IsNullOrWhiteSpace(UserName) || string.IsNullOrWhiteSpace(Password); 
        }

        private bool CheckPasswordHandler()
        {
            if(Password != string.Empty && CheckPassword != string.Empty) { 
                if (!Password.Equals(CheckPassword))
                {                  
                    PasswordColor = Colores.crimson.ToString();
                    return true;
                }
                else
                {
                    PasswordColor = Colores.lime.ToString();
                    return false;
                }
            }
            PasswordColor = Colores.white.ToString();
            return true;

        }

        private void CheckUserNameHandler()
        {
            if (CheckFormat(UserName))
                UserNameColor = Colores.lime.ToString();
            else            
                UserNameColor = Colores.white.ToString();
            
        }

        private void CheckNameHandler()
        {         
            if (CheckFormat(Name))
                NameColor = Colores.lime.ToString();
            else
                NameColor = Colores.white.ToString();
        }

        private void CheckSurnameHandler()
        {
            if (CheckFormat(Surname))
                SurnameColor = Colores.lime.ToString();
            else
               SurnameColor = Colores.white.ToString();
        }

        private void CheckClaveHandler()
        {
            CheckFormat(Clave);
            if (CheckFormat(Clave))
                ClaveColor = Colores.lime.ToString();
            else
                ClaveColor = Colores.white.ToString();
        }
        #endregion Handlers

        private bool CheckFormat(string word)
        {
            if (string.IsNullOrWhiteSpace(word) || word.Length < 3)
            {
                ShowMessage(ToastType.Warning, "El nombre de usuario y la contraseña deben tener al menos 3 caracteres");
                return false;
            }
            return true;
        }

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
