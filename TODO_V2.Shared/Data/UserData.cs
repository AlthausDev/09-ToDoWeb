using TODO_V2.Shared.Models;

namespace BlazorWebPage.Shared.Data
{
    public class UserData
    {
        public static User[] users { get; set; }
        static UserData()
        {
            cargarDatos();
        }

        private static void cargarDatos()
        {
            users = new User[] {
                new(1,"john_doe", "password123"),
                new(2,"jane_smith", "abc123", "Jane Smith", "jane@example.com", "01-01-2024"),
                new(3,"admin", "adminpass","Admin User","admin@example.com", "01-10-2024"),
                new(4,"testuser", "test123","Test User","test@example.com", "01-02-2024"),
                new(5,"newuser", "newpass"),
                new(6,"user123", "userpass","User 123","user123@example.com","01-09-2024"),
                new(7,"admin2", "password","Admin 2","admin2@example.com","01-03-2024"),
                new(8,"guest", "welcome123","guest 2","guest@example.com","01-05-2024"),
                new(9,"developer", "devpass","Developer User","dev@example.com","01-02-2024"),
                new(10,"manager", "managerpass","Manager User","manager@example.com","01-03-2024"),
                new(11,"admin3", "pass123","Admin 3","admin3@example.com","01-06-2024"),
                new(12,"user456", "user456pass"),
                new(13,"guest2", "guestpass","Guest User","guest@example.com","01-08-2024"),
                new(14,"testuser2", "testing123","testing 123","testing123@example.com","01-12-2024"),
                new(15,"user789", "userpass789","User 789","user789@example.com","01-06-2024"),
                new(16,"developer2", "devpass2","Developer 2","dev2@example.com","01-07-2024"),
                new(17,"admin4", "adminpass123","Admin 4","admin4@example.com","01-10-2024"),
                new(18,"user10", "userpass10","user 10","user10@example.com","01-05-2024"),
                new(19,"guest3", "guestpass3","Guest 3","guest3@example.com","01-11-2024"),
                new(20,"tester", "testpass","Tester User","tester@example.com","01-11-2024")
            };

        }

    }
}
