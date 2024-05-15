﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;

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
        public DateOnly FechaRegistro { get; set; } = DateOnly.FromDateTime(DateTime.Now);


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

        public override string ToString()
        {         
            Debug.WriteLine($"Id: {Id}" +
               $"\nUsername: {UserName}" +
               $"\nContraseña: {Password}" +
               $"\nNombre: {Name}" +
               $"\nApellido: {Surname}" +
               $"\nFecha de Registro: {FechaRegistro}");
            return "";

        }
    }
}
