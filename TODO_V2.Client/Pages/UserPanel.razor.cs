using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using static System.Net.WebRequestMethods;

namespace TODO_V2.Client.Pages
{
    partial class UserPanel
    {
        public static Modal ModalInstance = default!;

        [Parameter]
        public string Id { get; set; }


        private async Task OnClickLogOut()
        {
            var response = await Http.DeleteAsync("/api/User/logout");

            await storageService.RemoveItemAsync("token");
            NavManager.NavigateTo("/");
            Http.DefaultRequestHeaders.Remove("Authorization");
        }

        public static async Task HideModal()
        {
            await ModalInstance.HideAsync();
        }   
    }
}
