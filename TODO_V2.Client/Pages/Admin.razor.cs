using Azure;
using System.Reflection.Metadata;
using TODO_V2.Client.Layout;
using TODO_V2.Shared.Models;
using static System.Net.WebRequestMethods;

namespace TODO_V2.Client.Pages
{
    partial class Admin
    {

        private string ShowUsersMannager = "none";
        private string ShowCategoriesMannager = "block";

        private void OnClickShowUsers()
        {
            ShowCategoriesMannager = "none";
            ShowUsersMannager = "block";
        }

        private void OnClickShowCategories()
        {
            ShowUsersMannager = "none";
            ShowCategoriesMannager = "block";
        }

        private async Task OnClickLogOut()
        {
   
                var response = await Http.DeleteAsync("/api/User/logout");

                await storageService.RemoveItemAsync("token");
                NavManager.NavigateTo("/");
                Http.DefaultRequestHeaders.Remove("Authorization");
        }
    }
}
