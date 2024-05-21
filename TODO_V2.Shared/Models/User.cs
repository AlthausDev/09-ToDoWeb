using System;
using System.Diagnostics;
using TODO_V2.Shared.Models.Enum;

namespace TODO_V2.Shared.Models
{
    public class User : BaseModel
    {
        private string _userName;
        public string UserName
        {
            get => _userName;
            set => _userName = value.ToUpper();
        }

        public string Surname { get; set; }        
        public string UserType { get; set; }


        public User() { }

        public User(string userName, string name, string surname, string userType)
        {
            Name = name;
            Surname = surname;
            UserName = userName;
            UserType = userType;
        }

        public override string ToString()
        {
            Debug.WriteLine($"Id: {Id}" +
               $"\nUsername: {UserName}" +
               $"\nNombre: {Name}" +
               $"\nApellido: {Surname}" +
               $"\nFecha de Registro: {CreatedAt}");
            return "";
        }
    }
}
