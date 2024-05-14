using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public class User
    {
        private string? taskName;
        private string v;

        public User()
        {
        }

        public User(string userName, string password, string? taskName, string? userType, string v)
        {
            UserName = userName;
            Password = password;
            this.taskName = taskName;
            UserType = userType;
            this.v = v;
        }

        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }           
    }
}
