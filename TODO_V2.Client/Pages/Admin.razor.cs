using BlazorBootstrap;
using Microsoft.AspNetCore.Components;
using System.Diagnostics;

namespace TODO_V2.Client.Pages
{
    partial class Admin
    {
        public static Modal ModalInstance = default!;

        [Parameter]
        public string Id { get; set; }

        private string ShowUsersMannager = "none";
        private string ShowCategoriesMannager = "block";
        private string ShowTodoList = "none";

        private void OnClickShowUsers()
        {
            ShowUsersMannager = "block";
            ShowCategoriesMannager = "none";            
            ShowTodoList = "none";
        }

        private void OnClickShowCategories()
        {
            ShowUsersMannager = "none";
            ShowCategoriesMannager = "block";
            ShowTodoList = "none";
        }

        private void OnClickShowTodoList()
        {
            Debug.WriteLine(Id);

            ShowUsersMannager = "none";
            ShowCategoriesMannager = "none";
            ShowTodoList = "block";
        }

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
