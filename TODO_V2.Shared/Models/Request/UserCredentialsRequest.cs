using System;
using System.Collections.Generic;
using System.Text;
using TODO_V2.Client.DTO;

namespace TODO_V2.Shared.Models.Request
{
    public class UserCredentialsRequest
    {
        public User user { get; set; }
        public LoginCredentials Credentials { get; set; }

        public UserCredentialsRequest(User user, LoginCredentials credentials)
        {
            this.user = user;
            Credentials = credentials;
        }
    }

}
