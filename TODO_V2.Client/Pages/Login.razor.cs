using BlazorBootstrap;
using TODO_V2.Client.Shared.Modals;
using TODO_V2.Shared;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using ToastType = BlazorBootstrap.ToastType;
using Blazored.LocalStorage;
using Microsoft.JSInterop;
using TODO_V2.Shared.Models;
using TODO_V2.Client.Shared;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using TODO_V2.Shared.Model;

namespace TODO_V2.Client.Pages
{
    public partial class Login
    {
    
        private Modal ModalInstance = default!;
        private string? Message = string.Empty;
        private User user = new();    

        private string UserName { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            try
            {
               
            }
            catch { }
        }


        private void IniciarSesion()
        {
            NavManager.NavigateTo($"/admin/", false);
        }

        #region Registro
        private async Task Registro()
        {
            var parameters = new Dictionary<string, object>
    {
        { "Registrar", EventCallback.Factory.Create<MouseEventArgs>(this, NuevoUsuario) },
        { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
    };

            await ModalInstance.ShowAsync<ModalRegistro>(title: "Registrarse", parameters: parameters);
        }


        private async Task NuevoUsuario()
        {
            //bool existe = Usuarios.Any(user => user.UserName == NewUser.UserName || (user.Email == NewUser.Email && !string.IsNullOrEmpty(NewUser.Email)));

            //if (existe)
            //{
            //    ShowMessage(ToastType.Danger, "El nombre de usuario o email ya está registrado");
            //    await HideModal();
            //}
            //else
            //{
            //    await PostNewUser();
            //    ShowMessage(ToastType.Success, "Registro realizado con éxito");
            //    await IniciarSesion();
            //}
        }
        #endregion Registro             


        private async Task HideModal()
        {
            user = new User();
            await ModalInstance.HideAsync();
        }

        #region Handlers
        protected void OnClickLogin()
        {
            //User user = new(UserName, Password);
            //Home.NewUser = user;
            //Login.InvokeAsync();
        }

        protected void OnClickClose()
        {
            UserName = string.Empty;
            Password = string.Empty;
            //Cerrar.InvokeAsync();
        }


        #endregion Handlers


        private Task OnClickLogin(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }

        #region AuxMethods

        private async Task GenerateTokenAsync(User usuario)
        {
            HttpResponseMessage response = await Http.PostAsJsonAsync("user/login", usuario);
            if (response.IsSuccessStatusCode)
            {
                TokenResponse tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();

                await JS.InvokeVoidAsync("localStorage.setItem", new object[] { "token", tokenResponse.Token });
                Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {tokenResponse.Token}");
            }
            else
            {
                Console.WriteLine("Error al generar el token");
            }
        }

        #endregion
    }
}
