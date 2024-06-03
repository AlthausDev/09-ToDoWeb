using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TODO_V2.Client.Data;
using TODO_V2.Shared.Data;


namespace TODO_V2.Client.Pages
{
    public partial class StartUp
    {

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await LoadTestDataIfNeeded();
            await CategoryDictionary.LoadCategoryDictionary(Http);
            await CheckToken();
        }

        #region Load Data
        private async Task LoadTestDataIfNeeded()
        {
            if (!await ExistAnyUser())
                await UserData.LoadTestUsers(Http);

            if (!await ExistAnyCategory())
                await CategoryData.LoadTestCategories(Http);

            if (!await ExistAnyTask())
                await TaskItemData.LoadTestTasks(Http);
        }
        #endregion

        #region Data Existence Checks 
        private async Task<bool> ExistAnyUser()
        {
            return await ExistAnyElement("api/User/count", "User");
        }

        private async Task<bool> ExistAnyCategory()
        {
            return await ExistAnyElement("api/Category/count", "Category");
        }

        private async Task<bool> ExistAnyTask()
        {
            return await ExistAnyElement("api/TaskItem/count", "TaskItem");
        }

        private async Task<bool> ExistAnyElement(string endpoint, string elementName)
        {
            try
            {
                int elementCount = await Http.GetFromJsonAsync<int>(endpoint);
                return elementCount > 0;
            }
            catch (Exception ex)
            {
                // Debug.WriteLine($"Error al verificar la existencia de {elementName}: {ex.Message}");
                return false;
            }
        }

        #endregion

        private async Task CheckToken()
        {
            try
            {
                string getToken = await storageService.GetItemAsStringAsync("token");

                if (string.IsNullOrEmpty(getToken))
                {
                    NavManager.NavigateTo("/login");
                    return;
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(getToken);
                List<Claim> claims = jwtSecurityToken.Claims.ToList();

                int userId = int.Parse(claims.ElementAt(0).Value);
                string UserType = claims.ElementAt(2).Value;

                NavigateBasedOnUserType(UserType, userId);
            }
            catch (Exception)
            {
                Http.DefaultRequestHeaders.Remove("Authorization");
                NavManager.NavigateTo("/login");
            }
        }

        private void NavigateBasedOnUserType(string UserType, int Id)
        {
            switch (UserType)
            {
                case "USUARIO":
                    NavManager.NavigateTo($"/user/{Id}");
                    break;
                case "ADMINISTRADOR":
                    NavManager.NavigateTo($"/admin/{Id}");
                    break;
                default:
                    break;
            }
        }

    }
}
