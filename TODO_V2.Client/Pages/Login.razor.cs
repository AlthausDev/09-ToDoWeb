using BlazorBootstrap;
using Blazored.Modal;
using TODO_V2.Shared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TODO_V2.Client.DTO;
using TODO_V2.Client.Layout;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Utils;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Client.Pages
{
    public partial class Login 
    {
        
        public static User user = new();

        public Modal ModalInstance = default!;
        List<ToastMessage> messages = new();

        private string UserName { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;        


        #region Login     
        private async Task OnClickLogin()
        {
            var loginResult = await LoginUser(UserName, Password);
            LoginResult(loginResult);
        }

        private void LoginResult(ActionResult<User> loginResult)
        {
            try
            {
                if (loginResult == null || loginResult.Value == null)
                {
                    ShowMessage(ToastType.Danger, "Credenciales incorrectas. Por favor, inténtelo de nuevo.");
                    return;
                }

                NavigateBasedOnUserType(loginResult.Value);
            }
            catch (Exception ex)
            {
                ShowMessage(ToastType.Danger, $"Ocurrió un error al procesar el inicio de sesión: {ex.Message}");
            }
        }

        private void NavigateBasedOnUserType(User user)
        {
            switch (user.UserType)
            {
                case "USUARIO":
                    NavManager.NavigateTo($"/todo/{user.Id}");
                    break;
                case "ADMINISTRADOR":
                    NavManager.NavigateTo("/admin");
                    break;
                default:
                    ShowMessage(ToastType.Danger, $"Tipo de usuario desconocido: {user.UserType}");
                    break;
            }
        }
        #endregion

        #region Register
        private async Task OnClickRegister()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Registrar", EventCallback.Factory.Create<MouseEventArgs>(this, Registro) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalRegistro>(title: "Registrarse", parameters: parameters);
        }


        private async Task Registro()
        {
            ShowMessage(ToastType.Success, "El Registro se ha realizado exitosamente");
            await HideModal();
        }

        private async Task HideModal()
        {
            user = new User();
            await ModalInstance.HideAsync();
        }
        #endregion Register

        #region Api            
        private async Task<ActionResult<User>> LoginUser(string Username, string Password)
        {
            try
            {         
                var credentials = new LoginCredentials(Username, Password );
                var response = await Http.PostAsJsonAsync("api/User/login", credentials);

                if (response.IsSuccessStatusCode)
                {
                    LoginResponse loginResponse = await response.Content.ReadFromJsonAsync<LoginResponse>();
                    await GenerateTokenAsync(loginResponse.Token);                    
                    return new ActionResult<User>(loginResponse.User);
                }
                else
                {
                    return new ActionResult<User>(new NotFoundResult());
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error al intentar iniciar sesión: {ex.Message}");
                return new ActionResult<User>(new StatusCodeResult(500));
            }
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

        #region Token
        private async Task GenerateTokenAsync(string token)
        {
            await JS.InvokeVoidAsync("localStorage.setItem", new object[] { "token", token });
            Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {token}");
        }
        #endregion Token   
    }
}
