using BlazorBootstrap;
using TODO_V2.Client.Pages;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using TODO_V2.Shared.Models.Enum;
using TODO_V2.Shared.Utils;
using static System.Net.WebRequestMethods;
using TODO_V2.Shared.Models;
using System.Net.Http.Json;
using System.Diagnostics;
using TODO_V2.Client.DTO;
using TODO_V2.Shared.Models.Request;

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
        private string UserType { get; set; } = UserTypeEnum.USUARIO.ToString();

        private User? NewUser;
        private LoginCredentials? Credentials { get; set; }

        private string? PasswordColor, UserNameColor, NameColor, SurnameColor, ClaveColor;
        private bool IsInputValid = false;

        List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Registrar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        #region OnClick
        protected async Task OnClickRegistro()
        {
            bool isPasswordValid = CheckPasswordHandler();
            bool isUserNameValid = CheckUserNameHandler();
            bool isNameValid = CheckNameHandler();
            bool isSurnameValid = CheckSurnameHandler();
            bool isClaveValid = CheckClaveHandler();

            if (!isPasswordValid || !isUserNameValid || !isNameValid || !isSurnameValid || !isClaveValid)
            {
                ShowMessage(ToastType.Danger, "Por favor, complete todos los campos correctamente.");
                if (!isPasswordValid) PasswordColor = ColorsEnum.crimson.ToString();
                if (!isUserNameValid) UserNameColor = ColorsEnum.crimson.ToString();
                if (!isNameValid) NameColor = ColorsEnum.crimson.ToString();
                if (!isSurnameValid) SurnameColor = ColorsEnum.crimson.ToString();
                if (!isClaveValid) ClaveColor = ColorsEnum.crimson.ToString();
                return;
            }

            if (!IsInputValid)
            {
                ShowMessage(ToastType.Danger, "Los datos introducidos no son correctos.");
                return;
            }

            NewUser = new(UserName, Name, Surname, UserTypeEnum.USUARIO.ToString());
            Credentials = new(UserName, Password);

            if (await RegisterUser())
            {
                Login.user = NewUser;
                ClearFields();
                await Registrar.InvokeAsync();
            }
            else
            {
                ShowMessage(ToastType.Danger, "El Username introducido ya existe. Por favor, introduzca un nuevo Username");
                UserNameColor = ColorsEnum.crimson.ToString();
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
            bool isPasswordValid = CheckPasswordHandler();
            bool isUserNameValid = CheckUserNameHandler();
            bool isNameValid = CheckNameHandler();
            bool isSurnameValid = CheckSurnameHandler();

            IsInputValid = isPasswordValid && isUserNameValid && isNameValid && isSurnameValid;
            CheckClaveHandler();
        }

        private bool CheckPasswordHandler()
        {
            if (string.IsNullOrEmpty(Password) || string.IsNullOrEmpty(CheckPassword))
            {
                PasswordColor = ColorsEnum.white.ToString();
                return false;
            }

            if (!CheckFieldFormat(Password, FieldTypeEnum.AlphaNumeric.ToString(), ref PasswordColor))
            {
                return false;
            }

            bool passwordsMatch = Password.Equals(CheckPassword);
            PasswordColor = passwordsMatch ? ColorsEnum.lime.ToString() : ColorsEnum.crimson.ToString();
            return passwordsMatch;
        }


        private bool CheckUserNameHandler()
        {
            return CheckFieldFormat(UserName, FieldTypeEnum.AlphaNumeric.ToString(), ref UserNameColor);
        }

        private bool CheckNameHandler()
        {
            return CheckFieldFormat(Name, FieldTypeEnum.Alphabetical.ToString(), ref NameColor);
        }

        private bool CheckSurnameHandler()
        {
            return CheckFieldFormat(Surname, FieldTypeEnum.Alphabetical.ToString(), ref SurnameColor);
        }

        private bool CheckClaveHandler()
        {
            if (Validation.CheckKey(Clave))
            {
                return true;
            }
            else
            {
                ClaveColor = ColorsEnum.white.ToString();
                return false;
            }
        }

        #endregion Handlers

        #region Api
        private async Task<bool> RegisterUser()
        {
            try
            {
                UserCredentialsRequest request = new(NewUser, Credentials);

                HttpResponseMessage response = await Http.PostAsJsonAsync("user", request);
                response.EnsureSuccessStatusCode();

                var data = await response.Content.ReadAsStringAsync();               
                return data.Equals("true");
            }
            catch (HttpRequestException)
            {
                ClearFields();
                return false;
            }
        }

        private async Task<User?> GetUserByUserName(string Username)
        {
            try
            {
                return await Http.GetFromJsonAsync<User>($"user/{Username}");
            }
            catch (HttpRequestException)
            {               
                return null;
            }
        }

        //private async Task<bool> RegisterUser()
        //{
        //    HttpResponseMessage response = await Http.PostAsJsonAsync("user", NewUser);
        //    var data = await response.Content.ReadAsStringAsync();

        //    if (data.Equals("false"))
        //    {
        //        return false;
        //    }

        //    ClearFields();
        //    return true;
        //}
        
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
            UserName = Password = CheckPassword = Name = Surname = Clave = string.Empty;
            PasswordColor = UserNameColor = NameColor = SurnameColor = ClaveColor = ColorsEnum.white.ToString();
        }
        #endregion Aux
    }
}
