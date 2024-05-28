using BlazorBootstrap;

namespace TODO_V2.Client.Pages
{
    partial class Admin
    {

        public static Modal ModalInstance = default!;

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

        public static async Task HideModal()
        {       
            await ModalInstance.HideAsync();
        }
    }
}
