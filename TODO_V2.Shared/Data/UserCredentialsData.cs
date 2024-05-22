using System.Net.Http.Json;
using System.Threading.Tasks;
using TODO_V2.Client.DTO;
using TODO_V2.Server.Models;
using TODO_V2.Shared.Models;

namespace TODO_V2.Shared.Data
{
    public class UserCredentialsData
    {
        public static UserCredentials[] UsersCredentials { get; set; }
        public static LoginCredentials[] Credentials { get; set; }

        public static async Task LoadTestCredentials(HttpClient http)
        {
            UsersCredentials =[            
                new(1,"Admin", "111"),
                new(2,"jane", "111"),
                new(3,"mike", "222"),
                new(4,"sarah", "333"),
                new(5,"david", "444"),
                new(6,"emily", "555"),
                new(7,"tom", "666"),
                new(8,"laura", "777"),
                new(9,"chris", "888"),
                new(10,"rachel", "9999"),
                new(11,"emma", "1234"),
                new(12,"will", "2345"),
                new(13,"olivia", "3456"),
                new(14,"james", "4567"),
                new(15,"isabella", "5678"),
                new(16,"alex", "6789"),
                new(17,"sophia", "7890"),
                new(18,"ben", "8901"),
                new(19,"amelia", "9012"),
                new(20,"mia", "0123"),
                new(21,"aaa", "aaa")
            ];

            Credentials = [
               new("Admin", "111"),
                new("jane", "111"),
                new("mike", "222"),
                new("sarah", "333"),
                new("david", "444"),
                new("emily", "555"),
                new("tom", "666"),
                new("laura", "777"),
                new("chris", "888"),
                new("rachel", "9999"),
                new("emma", "1234"),
                new("will", "2345"),
                new("olivia", "3456"),
                new("james", "4567"),
                new("isabella", "5678"),
                new("alex", "6789"),
                new("sophia", "7890"),
                new("ben", "8901"),
                new("amelia", "9012"),
                new("mia", "0123"),
                new("aaa", "aaa")
           ];          
        }
    }
}
