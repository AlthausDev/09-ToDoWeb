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
using System.Threading.Tasks;
using Fare;
using Microsoft.JSInterop;

namespace TODO_V2.Client.Shared.Modals
{
    partial class ModalRegistro
    {
        [Parameter]
        public int? Id { get; set; }  
        public string CheckPassword { get; set; } = string.Empty;
        public string Clave { get; set; } = string.Empty;   
        public string UserType { get; set; } = UserTypeEnum.USUARIO.ToString();


        private User? NewUser = new();
        private LoginCredentials? Credentials { get; set; } = new();

        private string? PasswordColor = "#03e9f4";
        private string? UserNameColor = "#03e9f4";
        private string? NameColor = "#03e9f4";
        private string? SurnameColor = "#03e9f4";
        private string? ClaveColor = "#03e9f4";

        private bool IsInputValid = false;
        public bool IsEditing { get; private set; } = false;

        [Parameter]
        public bool IsAdminDisplay {get; set; } = false;


        List<ToastMessage> messages = new();

        [Parameter] public EventCallback<MouseEventArgs> Aceptar { get; set; }
        [Parameter] public EventCallback<MouseEventArgs> Cerrar { get; set; }


        protected override async Task OnParametersSetAsync()
        {
            if (Id.HasValue)
            {
                IsEditing = true;
                IsInputValid = true;
                await LoadUserById(Id.Value);                
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
            if (IsEditing)
            {
                Credentials.Username = NewUser.UserName;
                await EditUser();
                await Aceptar.InvokeAsync();
            }
            else
            { 
                Credentials = new(NewUser.UserName, Credentials.Password);

                if (await RegisterUser())
                {
                    Login.user = NewUser;
                    ClearFields();
                    await Aceptar.InvokeAsync();
                }
                else
                {
                    ShowMessage(ToastType.Danger, "El Username introducido ya existe. Por favor, introduzca un nuevo Username");
                    UserNameColor = ColorsEnum.crimson.ToString();
                }
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
            if (!IsAdminDisplay)
            {
                if (string.IsNullOrEmpty(Credentials.Password) || string.IsNullOrEmpty(CheckPassword))
                {
                    PasswordColor = "#03e9f4";
                    return false;
                }

                if (!CheckFieldFormat(Credentials.Password, FieldTypeEnum.AlphaNumeric.ToString(), ref PasswordColor))
                {
                    return false;
                }
            }
           
            Debug.WriteLine(Credentials.Password);
            Debug.WriteLine(CheckPassword);

            bool passwordsMatch = Credentials.Password.Equals(CheckPassword);
            PasswordColor = passwordsMatch ? ColorsEnum.lime.ToString() : ColorsEnum.crimson.ToString();
            return passwordsMatch;
        }


        private bool CheckUserNameHandler()
        {
            return CheckFieldFormat(NewUser.UserName, FieldTypeEnum.AlphaNumeric.ToString(), ref UserNameColor);
        }

        private bool CheckNameHandler()
        {
            return CheckFieldFormat(NewUser.Name, FieldTypeEnum.Alphabetical.ToString(), ref NameColor);
        }

        private bool CheckSurnameHandler()
        {
            return CheckFieldFormat(NewUser.Surname, FieldTypeEnum.Alphabetical.ToString(), ref SurnameColor);
        }

        private bool CheckClaveHandler()
        {
            if (!IsAdminDisplay)
            {
                if (Validation.CheckKey(Clave))
                {
                    return true;
                }
                else
                {
                    ClaveColor = "#03e9f4";
                    return false;
                }
            }
            return true;
        }

        #endregion Handlers

        #region Api
        private async Task<bool> RegisterUser()
        {
            try
            {
                UserCredentialsRequest request = new(NewUser, Credentials);

                HttpResponseMessage response = await Http.PostAsJsonAsync("api/User", request);
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

        private async Task<bool> EditUser()
        {
            try
            {
                UserCredentialsRequest request = new(NewUser, Credentials);

                HttpResponseMessage response = await Http.PutAsJsonAsync($"api/User", request);
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
        #endregion Api

        private async Task LoadUserById(int UserId)
        {
            if (UserId != 0)
            {
                try
                {
                    User user = await Http.GetFromJsonAsync<User>($"api/User/{UserId}");                     

                    if (user != null)
                    {
                        await LoadCredentialsById(user.Id);
                        Debug.WriteLine(Credentials.Password);
                        NewUser = user;   
                    }
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error al cargar la tarea por Id: {ex.Message}");
                }
            }
            else
            {
                NewUser = new User();
            }
        }

        private async Task LoadCredentialsById(int userId)
        {
            try
            {
                Credentials = await Http.GetFromJsonAsync<LoginCredentials>($"api/User/credentials/{userId}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al cargar las credenciales del usuario: {ex.Message}");              
            }
        } 

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
            fieldColor = isValid ? ColorsEnum.lime.ToString() : "#03e9f4";
            return isValid;
        }

        private void ClearFields()
        {
            NewUser.UserName = Credentials.Password = CheckPassword = NewUser.Name = NewUser.Surname = Clave = string.Empty;
            NewUser.UserType = UserTypeEnum.USUARIO.ToString();
            PasswordColor = UserNameColor = NameColor = SurnameColor = ClaveColor = "#03e9f4";
        }
        #endregion Aux
    }
}
