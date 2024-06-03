using TODO_V2.Client.ClienteModels;

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
