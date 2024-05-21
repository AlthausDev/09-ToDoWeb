using TODO_V2.Shared.Models;

namespace TODO_V2.Server.Models
{
    public class UserCredentials : BaseModel
    {
        public int UserId { get; set; }

        private string _userName;
        public string UserName
        {
            get => _userName;
            set => _userName = value.ToUpper(); 
        }
        public string EncryptedPassword { get; set; }

        public UserCredentials()
        {
        }

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
