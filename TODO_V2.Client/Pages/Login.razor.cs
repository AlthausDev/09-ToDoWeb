using BlazorBootstrap;
using TODO_V2.Client.Shared.Modals;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Web;
using System.Net.Http.Json;
using Microsoft.JSInterop;
using TODO_V2.Shared.Models;
using System.Diagnostics;
using BlazorWebPage.Shared.Data;
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


        //protected override async Task OnInitializedAsync()
        //{
        //    base.OnInitialized();

        //    Debug.WriteLine(await GetUserById(1));
        //    Debug.WriteLine(await GetUserById(1));
        //    Debug.WriteLine(await GetUserById(1));
        //    Debug.WriteLine(await GetUserById(1));
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Si no hay usuarios, cargarlos
            try
            {
                Debug.WriteLine(await GetUserById(1));
            }
            catch
            {
                await UserData.CargarDatosAsync(Http);

            }
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
        private async Task GetData()
        {
            User[]? usuariosArray = await Http.GetFromJsonAsync<User[]>("user");
            if (usuariosArray != null)
            {
                List<User> Usuarios = [.. usuariosArray];
                foreach (User user in Usuarios)
                {
                    //Debug.WriteLine(user.ToString);
                }
            }
        }

        private async Task<User?> GetUserById(int id)
        {
            return await Http.GetFromJsonAsync<User>($"user/{id}");
        }

        private async Task<User?> GetUserByUserName(string Username)
        {
            return await Http.GetFromJsonAsync<User>($"user/{Username}");
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
        #endregion Aux   

        #region Handlers    
        #endregion Handlers
    }
}
