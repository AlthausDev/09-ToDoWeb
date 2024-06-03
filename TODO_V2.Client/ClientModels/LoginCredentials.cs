namespace TODO_V2.Client.ClientModels
{
    public class LoginCredentials
    {
        private string _username;

        public string Username
        {
            get => _username;
            set => _username = value.ToUpper();
        }

        public string Password { get; set; }

        public LoginCredentials(string username, string password)
        {
            Username = username;
            Password = password;
        }

        public LoginCredentials()
        {
        }
    }
}
