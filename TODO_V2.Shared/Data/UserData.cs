using System.Net.Http.Json;
using System.Threading.Tasks;
using TODO_V2.Server.Models;
using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using static System.Net.WebRequestMethods;
using TODO_V2.Shared.Models.Request;

namespace TODO_V2.Shared.Data
{
    public class UserData
    {
        public static User[] Users { get; set; }
        

        public static async Task LoadTestUsers(HttpClient http)
        {
            await UserCredentialsData.LoadTestCredentials(http);

            Users = [            
                new("Admin", "John", "Doe", UserTypeEnum.ADMINISTRADOR.ToString()),
                new("jane", "Jane", "Smith", UserTypeEnum.USUARIO.ToString()),
                new("mike", "Mike", "Johnson", UserTypeEnum.USUARIO.ToString()),
                new("sarah", "Sarah", "Williams", UserTypeEnum.USUARIO.ToString()),
                new("david", "David", "Brown", UserTypeEnum.USUARIO.ToString()),
                new("emily", "Emily", "Jones", UserTypeEnum.USUARIO.ToString()),
                new("tom", "Tom", "Wilson", UserTypeEnum.USUARIO.ToString()),
                new("laura", "Laura", "Davis", UserTypeEnum.USUARIO.ToString()),
                new("chris", "Chris", "Moore", UserTypeEnum.USUARIO.ToString()),
                new("rachel", "Rachel", "Taylor", UserTypeEnum.USUARIO.ToString()),         
                new("emma", "Emma", "White", UserTypeEnum.USUARIO.ToString()),
                new("will", "William", "Wilson", UserTypeEnum.USUARIO.ToString()),
                new("olivia", "Olivia", "Brown", UserTypeEnum.USUARIO.ToString()),
                new("james", "James", "Jones", UserTypeEnum.USUARIO.ToString()),
                new("isabella", "Isabella", "Taylor", UserTypeEnum.USUARIO.ToString()),
                new("alex", "Alexander", "Martinez", UserTypeEnum.USUARIO.ToString()),
                new("sophia", "Sophia", "Anderson", UserTypeEnum.USUARIO.ToString()),
                new("ben", "Benjamin", "Davis", UserTypeEnum.USUARIO.ToString()),
                new("amelia", "Amelia", "Garcia", UserTypeEnum.USUARIO.ToString()),
                new("mia", "Mia", "Rodriguez", UserTypeEnum.USUARIO.ToString()),
                new("aaa", "Test", "User", UserTypeEnum.USUARIO.ToString())
            ];

            for (int i = 0; i < Users.Length; i++)
            {
                UserCredentialsRequest request = new(Users[i], UserCredentialsData.Credentials[i]);
                HttpResponseMessage response = await http.PostAsJsonAsync("api/User", request);
            }            
        }
    }
}
