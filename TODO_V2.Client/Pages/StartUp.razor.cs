using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using System.Text;
using System.Text.Json;
using TODO_V2.Client.Data;
using TODO_V2.Shared.Data;
using TODO_V2.Shared.Models;


namespace TODO_V2.Client.Pages
{
    public partial class StartUp
    {

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //await InsertLog();
            await LoadTestDataIfNeeded();
            await CheckToken();
        }

        private async Task InsertLog()
        {
            var log = new Log();
            log.MachineName = Environment.MachineName;
            log.Logged = DateTime.Now;
            log.Level = "Information";
            log.Message = "Application started";
            log.Logger = "TODO_V2.Client.Pages.StartUp";
            log.Request_method = "GET";
            log.Stacktrace = null;
            log.File_name = "StartUp.razor.cs";
            log.All_event_properties = null;

            try
            {
                Debug.WriteLine("Sending log to server...");
                var response = await Http.PostAsJsonAsync("https://localhost:7216/api/log", log);

                if (response.IsSuccessStatusCode)
                {
                    Debug.WriteLine("Log inserted successfully.");
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Debug.WriteLine($"Error inserting log: {response.StatusCode} - {errorContent}");
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine($"Exception: {ex.Message}");
            }
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
            catch (Exception)
            {
                return false;
            }
        }

        #endregion

        private async Task CheckToken()
        {
            int? userId = null;
            string? UserType = null;

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

                userId = int.Parse(claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
                UserType = claims.FirstOrDefault(c => c.Type == ClaimTypes.Role)?.Value;                
            }
            catch (Exception)
            {
                _ = Http.DefaultRequestHeaders.Remove("Authorization");               
            }
            finally
            {
                await NavigateBasedOnUserType(UserType, userId);
            }
        }

        private async Task NavigateBasedOnUserType(string? UserType, int? Id)
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
                    NavManager.NavigateTo("/login");
                    break;
            }
        }

    }
}
