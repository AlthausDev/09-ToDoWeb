using BlazorBootstrap;
using TODO_V2.Shared.Data;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TODO_V2.Shared.Data;


namespace TODO_V2.Client.Pages
{
    public partial class StartUp
    {      

        //protected override async Task OnInitializedAsync()       
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            await LoadTestDataIfNeeded();
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
            return await ExistAnyElement("user/count", "users");
        }

        private async Task<bool> ExistAnyTask()
        {
            return true;
            return await ExistAnyElement("tasks/count", "tasks");
        }

        private async Task<bool> ExistAnyCategory()
        {
            //return true;
            return await ExistAnyElement("api/Category/count", "categories");
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
                Debug.WriteLine($"Error al verificar la existencia de {elementName}: {ex.Message}");
                return false;
            }
        }

        #endregion

        private async Task CheckToken()
        {     
            try
            {
                string getToken = await storageService.GetItemAsStringAsync("token");
                Debug.WriteLine(getToken);

                if (string.IsNullOrEmpty(getToken))
                {
                    NavManager.NavigateTo("/login");
                    return;
                }

                var handler = new JwtSecurityTokenHandler();
                var jwtSecurityToken = handler.ReadJwtToken(getToken);
                List<Claim> claims = jwtSecurityToken.Claims.ToList();

                int userId = int.Parse(claims.ElementAt(0).Value);
                NavManager.NavigateTo($"/todo/{userId}");
            }
            catch (Exception)
            {
                Http.DefaultRequestHeaders.Remove("Authorization");
                NavManager.NavigateTo("/login");
            }
        }
    }
}
