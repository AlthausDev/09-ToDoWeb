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

namespace TODO_V2.Client.Pages
{
    public partial class Login
    {
        private User user = new();
        private bool IsDisabled = false;
        private string UserName;
        private string Password;
        private void IniciarSesion()
        {
            NavManager.NavigateTo($"/Admin/", false);
        }
        private Task OnClickLogin(MouseEventArgs e)
        {
            throw new NotImplementedException();
        }
        private void ValueChangeHandler(KeyboardEventArgs e)
        {
            throw new NotImplementedException();
        }
    }
}
