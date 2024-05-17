using BlazorBootstrap;
using BlazorWebPage.Shared.Data;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Mvc;
using Microsoft.JSInterop;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TODO_V2.Client.DTO;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Client.Pages
{
    public partial class Login
    {

        private Modal ModalInstance = default!;
        public static User user = new();

        List<ToastMessage> messages = new();

        private string UserName { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;


        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Si no hay usuarios, cargarlos
            //if (!await ExistAnyData())
            //    await UserData.CargarDatosAsync(Http); 
            await CheckToken();
        }

        #region Login     
        private async Task OnClickLogin()
        {
            var loginResult = await LoginUser(UserName, Password);            
            HandleLoginResult(loginResult);
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
        private async Task<bool> ExistAnyData()
        {   
            return await Http.GetFromJsonAsync<int>("user") > 0;
        }

        private async Task<ActionResult<User>> LoginUser(string Username, string Password)
        {
            try
            {
                var credentials = new LoginCredentials { Username = Username.ToUpper(), Password = Password };
                var response = await Http.PostAsJsonAsync("user/login", credentials);
                if (response.IsSuccessStatusCode)
                {
                    await GenerateTokenAsync();
                    var loggedUser = await response.Content.ReadFromJsonAsync<User>();
                    return new ActionResult<User>(loggedUser);
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

        #region Aux
        private async Task GenerateTokenAsync()
        {
            HttpResponseMessage response = await Http.PostAsJsonAsync("user/login", user);
            TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            await JS.InvokeVoidAsync("localStorage.setItem", new object[] { "token", tokenResponse.Token });
            Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.Token}");

            //HttpResponseMessage response = await Http.PostAsJsonAsync("user/login", usuario);
            //if (response.IsSuccessStatusCode)
            //{
            //    TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

            //    await JS.InvokeVoidAsync("localStorage.setItem", new object[] { "token", tokenResponse.Token });
            //    Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.Token}");
            //}
            //else
            //{
            //    Console.WriteLine("Error al generar el token");
            //}
        }

        //private async Task CheckToken()
        //{
        //    try
        //    {
        //        var response = await Http.GetAsync("user/CheckToken");
        //        if (response.IsSuccessStatusCode)
        //        {
        //           NavManager.NavigateTo("/todo");
        //        }               
        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine($"Error al comprobar el token: {ex.Message}");
        //    }
        //}

        private async Task CheckToken()
        {
            try
            {
                string getToken = await storageService.GetItemAsStringAsync("token");

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(getToken);
                List<Claim> claims = jwtSecurityToken.Claims.ToList();

                Console.WriteLine(claims);
                int Id = int.Parse(claims.ElementAt(0).Value);

                NavManager.NavigateTo("/todo");
            }
            catch (Exception ex)
            {
                Http.DefaultRequestHeaders.Remove("Authorization");
            }
        }

        private async Task GetToken()
        {
            try
            {
                string getToken = await storageService.GetItemAsStringAsync("token");

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(getToken);
                List<Claim> claims = jwtSecurityToken.Claims.ToList();

                Console.WriteLine(claims);
                int Id = int.Parse(claims.ElementAt(0).Value);

            }
            catch (Exception ex)
            {
                Http.DefaultRequestHeaders.Remove("Authorization");
            }
        }
        #endregion Aux   

        #region Handlers  
        private void HandleLoginResult(ActionResult<User> loginResult)
        {
            if (loginResult.Value != null)
            {
                NavManager.NavigateTo("/todo");
            }
            else
            {
                ShowMessage(ToastType.Danger, "Credenciales incorrectas. Por favor, inténtelo de nuevo.");
            }
        }
        #endregion Handlers
    }
}
