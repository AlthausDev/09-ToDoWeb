using TODO_V2.Shared.Models;
using TODO_V2.Shared.Models.Enum;
using static System.Net.WebRequestMethods;
using System.Net.Http.Json;


namespace BlazorWebPage.Shared.Data
{
    public class UserData
    {
        public static User[] Users { get; set; }

        public static async Task CargarDatosAsync(HttpClient http)
        {
            string UserType = UserTypeEnum.USUARIO.ToString();

            Users = [
                //new(Name, Surname, UserName, Password),
                new("John", "Doe", "Admin", "111", UserTypeEnum.ADMINISTRADOR.ToString()),
                new("Jane", "Smith", "jane", "111", UserType),
                new("Mike", "Johnson", "mike", "222", UserType),
                new("Sarah", "Williams", "sarah", "333", UserType),
                new("David", "Brown", "david", "444", UserType),
                new("Emily", "Jones", "emily", "555", UserType),
                new("Tom", "Wilson", "tom", "666", UserType),
                new("Laura", "Davis", "laura", "777", UserType),
                new("Chris", "Moore", "chris", "888", UserType),
                new("Rachel", "Taylor", "rachel", "9999", UserType),
                new("Liam", "Anderson", "liam", "0000", UserType)
            ];

            foreach (User User in Users)
            {
                User.UserName = User.UserName.ToUpper();
                await http.PostAsJsonAsync("user", User);
            }
        }

    }
}
