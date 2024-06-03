namespace TODO_V2.Client.Components
{
    using Blazored.LocalStorage;
    using Microsoft.AspNetCore.Components;
    using System.Net.Http;
    using System.Threading.Tasks;

    partial class Logout : ComponentBase
    {
        [Inject] protected HttpClient HttpClient { get; set; }
        [Inject] protected NavigationManager NavigationManager { get; set; }
        [Inject] protected ILocalStorageService LocalStorageService { get; set; }

        protected async Task LogoutAsync()
        {
            var response = await HttpClient.DeleteAsync("/api/User/logout");
            if (response.IsSuccessStatusCode)
            {
                await LocalStorageService.RemoveItemAsync("token");
                NavigationManager.NavigateTo("/");
                HttpClient.DefaultRequestHeaders.Remove("Authorization");
            }
        }
    }

}
