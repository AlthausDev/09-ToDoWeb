using BlazorBootstrap;
using TODO_V2.Client.Shared.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Model;
using System.Net.Http;
using System.Diagnostics;

namespace TODO_V2.Client.Pages
{
    public partial class Login
    {

        private Modal ModalInstance = default!;
        public static User user = new();

        private string UserName { get; set; } = string.Empty;
        private string Password { get; set; } = string.Empty;

        protected override async Task OnInitializedAsync()
        {
            //await GetUserById(1);
        }

        #region Login     
        private async Task OnClickLogin()
        {
            NavManager.NavigateTo($"/admin/", true);
        }
        #endregion Login


        #region Register
        private async Task OnClickRegister()
        {
            var parameters = new Dictionary<string, object>
            {
                { "Registrar", EventCallback.Factory.Create<MouseEventArgs>(this, NewUser) },
                { "Cerrar", EventCallback.Factory.Create<MouseEventArgs>(this, HideModal) }
            };
            await ModalInstance.ShowAsync<ModalRegistro>(title: "Registrarse", parameters: parameters);
        }


        private async Task NewUser()
        {
            user.ToString();
            //bool existe = Usuarios.Any(user => user.UserName == NewUser.UserName || (user.Email == NewUser.Email && !string.IsNullOrEmpty(NewUser.Email)));

            //if (existe)
            //{
            //    ShowMessage(ToastType.Danger, "El nombre de usuario o email ya está registrado");
            //await HideModal();
            //}
            //else
            //{
            //    await PostNewUser();
            //    ShowMessage(ToastType.Success, "Register realizado con éxito");
            //    await IniciarSesion();
            //}
        }

        private async Task HideModal()
        {
            user = new User();
            await ModalInstance.HideAsync();
        }
        #endregion Register


        #region Api       
        private async Task GetData()
        {
            User[]? usuariosArray = await Http.GetFromJsonAsync<User[]>("user");
            if (usuariosArray != null)
            {
                List<User> Usuarios = [.. usuariosArray];
                foreach (User user in Usuarios)
                {
                    Debug.WriteLine(user.ToString);
                }
            }
        }

        private async Task<User?> GetUserById(int id)
        {
            return await Http.GetFromJsonAsync<User>($"{id}");
        }

        private async Task PostNewUser()
        {
            try { 
            User newUser = new("aaa", "aaa", "aaa", "aaa", "USUARIO");
            HttpResponseMessage response = await Http.PostAsJsonAsync("user", newUser);
            await Http.PostAsJsonAsync("user", newUser);
            await Http.PostAsJsonAsync("Users", newUser);
            await Http.PostAsJsonAsync("usuarios", newUser);
            }
            catch (Exception ex)
            {
                Debug.WriteLine("\nMessage ---\n{0}", ex.Message);
                Debug.WriteLine("\nHelpLink ---\n{0}", ex.HelpLink);
                Debug.WriteLine("\nSource ---\n{0}", ex.Source);
                Debug.WriteLine("\nStackTrace ---\n{0}", ex.StackTrace);
                Debug.WriteLine("\nTargetSite ---\n{0}", ex.TargetSite);
            }
           
        }

        #endregion Api     

        #region Token

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

        #endregion Token

        #region Aux
        #endregion Aux   

        #region Handlers    
        #endregion Handlers
    }
}
