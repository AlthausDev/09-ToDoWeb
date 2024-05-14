using System;
using System.Collections.Generic;
using System.Text;

namespace TODO_V2.Shared.Models
{
    public class User
    {   
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public string UserType { get; set; }       
        
        public User() { }

        public User(string name, string surname, string userName, string password, string userType)
        {
            Name = name;
            Surname = surname;
            UserName = userName;
            Password = password;
            UserType = userType;
        }

        public User(int id, string name, string surname, string userName, string password, string userType)
        {
            Id = id;
            Name = name;
            Surname = surname;
            UserName = userName;
            Password = password;
            UserType = userType;
        }
    }
}
