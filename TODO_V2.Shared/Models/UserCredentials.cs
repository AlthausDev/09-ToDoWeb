namespace TODO_V2.Server.Models
{
    public class UserCredentials
    {
        public int UserId { get; set; }
        public string UserName { get; set; }
        public string EncryptedPassword { get; set; }

        public UserCredentials(string username, string encryptedPassword)
        {
            UserName = username;
            EncryptedPassword = encryptedPassword;
        }

        public UserCredentials(int userId, string username, string encryptedPassword)
        {
            UserId = userId;
            UserName = username;
            EncryptedPassword = encryptedPassword;
        }
    }
}
