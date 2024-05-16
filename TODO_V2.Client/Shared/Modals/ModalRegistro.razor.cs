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

        private string? PasswordColor, UserNameColor, NameColor, SurnameColor, ClaveColor;
        private bool IsInputValid = false;

        List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Registrar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        #region OnClick
        protected async Task OnClickRegistro()
        {
            if (CheckClaveHandler())
            {
                if (IsInputValid)
                {
                    try
                    {
                        User? userExist = await GetUserByUserName(UserName.ToUpper());
                        ShowMessage(ToastType.Danger, @"El Username introducido ya existe. 
                                                    Por favor, introduzca un nuevo Username");
                        UserNameColor = Colores.crimson.ToString();
                    }
                    catch
                    {
                        NewUser = new(Name, Surname, UserName.ToUpper(), Password, UserTypeEnum.USUARIO.ToString());
                        Login.user = NewUser;
                        await RegisterUser();
                        ClearFields();

                        await Registrar.InvokeAsync();

                    }
                }
                else
                {
                    ShowMessage(ToastType.Danger, "Los datos introducidos no son correctos.");
                }               
            }
            else
            {
                ShowMessage(ToastType.Danger, "La clave introducida es incorrecta.");
                ClaveColor = Colores.crimson.ToString();
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
                return true;
            }
            else
            {
                ClaveColor = Colores.white.ToString();
                //ClaveColor = Colores.crimson.ToString();
                return false;
            }
        }
        #endregion Handlers

        #region Api
        private async Task RegisterUser()
        {
            HttpResponseMessage response = await Http.PostAsJsonAsync("user", NewUser); 
            Debug.WriteLine(await Http.GetFromJsonAsync<User>($"user/{UserName}"));

            ClearFields();
        }

        private async Task<User?> GetUserByUserName(string Username)
        {
            return await Http.GetFromJsonAsync<User>($"user/{Username}");
        }
        #endregion Api

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

        #region Aux
        private void ClearFields()
        {
            UserName = string.Empty;
            Password = string.Empty;
            CheckPassword = string.Empty;
            Name = string.Empty;
            Surname = string.Empty;
            Clave = string.Empty;

            UserType = UserTypeEnum.USUARIO.ToString();
        }
        #endregion Aux
    }
}
