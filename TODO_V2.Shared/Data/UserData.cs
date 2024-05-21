using System.Net.Http.Json;
using System.Threading.Tasks;
using TODO_V2.Server.Models;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using static System.Net.WebRequestMethods;
using TODO_V2.Shared.Models.Request;

namespace BlazorWebPage.Shared.Data
{
    public class UserData
    {
        public static User[] Users { get; set; }
        

        public static async Task LoadTestUsers(HttpClient http)
        {
            await UserCredentialsData.LoadTestCredentials(http);

            Users = [            
                new("Admin", "John", "Doe", UserTypeEnum.ADMINISTRADOR),
                new("jane", "Jane", "Smith", UserTypeEnum.USUARIO),
                new("mike", "Mike", "Johnson", UserTypeEnum.USUARIO),
                new("sarah", "Sarah", "Williams", UserTypeEnum.USUARIO),
                new("david", "David", "Brown", UserTypeEnum.USUARIO),
                new("emily", "Emily", "Jones", UserTypeEnum.USUARIO),
                new("tom", "Tom", "Wilson", UserTypeEnum.USUARIO),
                new("laura", "Laura", "Davis", UserTypeEnum.USUARIO),
                new("chris", "Chris", "Moore", UserTypeEnum.USUARIO),
                new("rachel", "Rachel", "Taylor", UserTypeEnum.USUARIO),         
                new("emma", "Emma", "White", UserTypeEnum.USUARIO),
                new("will", "William", "Wilson", UserTypeEnum.USUARIO),
                new("olivia", "Olivia", "Brown", UserTypeEnum.USUARIO),
                new("james", "James", "Jones", UserTypeEnum.USUARIO),
                new("isabella", "Isabella", "Taylor", UserTypeEnum.USUARIO),
                new("alex", "Alexander", "Martinez", UserTypeEnum.USUARIO),
                new("sophia", "Sophia", "Anderson", UserTypeEnum.USUARIO),
                new("ben", "Benjamin", "Davis", UserTypeEnum.USUARIO),
                new("amelia", "Amelia", "Garcia", UserTypeEnum.USUARIO),
                new("mia", "Mia", "Rodriguez", UserTypeEnum.USUARIO),
                new("aaa", "Test", "User", UserTypeEnum.USUARIO)
            ];

            for (int i = 0; i < Users.Length; i++)
            {
                UserCredentialsRequest request = new(Users[i], UserCredentialsData.Credentials[i]);
                HttpResponseMessage response = await http.PostAsJsonAsync("user", request);
            }            
        }
    }
}
