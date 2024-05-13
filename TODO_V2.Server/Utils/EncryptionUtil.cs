using System.Security.Cryptography;
using System.Text;

namespace TODO_V2.Server.Utils
{
    public sealed class EncryptionUtil
    {
        private const string _secret = "fwr8734rf46ef84ser86f46se84fs598";

        public string Encrypt(string data)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(data);

            using(var encryptor = Aes.Create())
            {
                byte[] IV = new byte[15];
                new Random().NextBytes(IV);

                Rfc2898DeriveBytes pdb = new(_secret, IV);

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);


                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new(memoryStream, encryptor.CreateEncryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(clearBytes, 0, clearBytes.Length);

                        cryptoStream.Close();
                    }
                    string result = Convert.ToBase64String(IV) + Convert.ToBase64String(memoryStream.ToArray());
                    return result;
                }
            }            
        }

        public string Decrypt(string data)
        {            

            byte[] IV = Convert.FromBase64String(data[..20]);
            data = data[20..].Replace(" ", "+");
            byte[] cipherBytes = Convert.FromBase64String(data);

            using (var  encryptor = Aes.Create())
            {
                var pdb = new Rfc2898DeriveBytes(_secret, IV);

                encryptor.Key = pdb.GetBytes(32);
                encryptor.IV = pdb.GetBytes(16);

                using (MemoryStream memoryStream = new())
                {
                    using (CryptoStream cryptoStream = new(memoryStream, encryptor.CreateDecryptor(), CryptoStreamMode.Write))
                    {
                        cryptoStream.Write(cipherBytes, 0, cipherBytes.Length);

                        cryptoStream.Close();
                    }
                    string result = Encoding.Unicode.GetString(memoryStream.ToArray());

                    return result;
                }
            }
        }
    }
}
