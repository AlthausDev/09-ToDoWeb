using BlazorBootstrap;
using BlazorWebPage.Shared.Data;
using System.Diagnostics;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Json;
using System.Security.Claims;
using TODO_V2.Shared.Data;


namespace TODO_V2.Client.Pages
{
    public partial class StartUp
    {

        public static Modal ModalInstance = default!;
        public static List<ToastMessage> messages = new();


        //protected override async Task OnInitializedAsync()
        //{
        //    //Si no hay usuarios, cargarlos
        //    //NavManager.NavigateTo("/login");
        //    //if (!await ExistAnyData())
        //    //    await UserData.CargarDatosAsync(Http);
        //    await CheckToken();
        //}

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            //Si no hay usuarios, cargarlos
            if (!await ExistAnyUser())
            {
                await UserData.LoadTestUsers(Http);               
            }

            //Si no hay tareas generarlas y las categorías
            //if (!await ExistAnyTask())
            //{
            //    await TaskItemData.LoadTestTasks(Http);
            //    await CategoryData.LoadTestCategories(Http);
            //}

            await CheckToken();
        }

        private async Task<bool> ExistAnyUser()
        {
            return await Http.GetFromJsonAsync<int>("user/count") > 0;
        }

        private async Task<bool> ExistAnyTask()
        {
            return await Http.GetFromJsonAsync<int>("tasks/count") > 0;
        }

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
