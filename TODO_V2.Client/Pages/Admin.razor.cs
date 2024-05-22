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
        private string ShowCategoriesMannager = "none";

        protected override async Task OnInitializedAsync()
        {            
            Http.DefaultRequestHeaders.Remove("Authorization");
            Http.DefaultRequestHeaders.Add("Authorization", $"Bearer {await storageService.GetItemAsStringAsync("token")}");
        }

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

        private async Task OnClickExit()
        {
            //try
            //{
                var response = await Http.DeleteAsync("/api/User/logout");

                await storageService.RemoveItemAsync("token");
                NavManager.NavigateTo("/");
                Http.DefaultRequestHeaders.Remove("Authorization");

            //    if (response.IsSuccessStatusCode)
            //    {
            //        await storageService.RemoveItemAsync("token");    
            //        NavManager.NavigateTo("/");
            //    }
            //    else
            //    {
            //        Console.WriteLine("Error al intentar cerrar sesión.");
            //    }
            //}
            //catch (Exception ex)
            //{
            //    Console.WriteLine($"Error al intentar cerrar sesión: {ex.Message}");
            //}
        }





        private void ToCategoriesMannager()
        {
            NavManager.NavigateTo($"/Categories/", false);
        }

        private void ToUsersMannager()
        {
            NavManager.NavigateTo($"/Users/", false);
        }
    }
}
