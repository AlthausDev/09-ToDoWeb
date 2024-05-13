namespace TODO_V2.Client.Pages
{
    partial class Admin
    {

        private string ShowUsersMannager = "none";
        private string ShowCategoriesMannager = "none";

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
