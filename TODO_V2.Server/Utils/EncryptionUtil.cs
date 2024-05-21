using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using TODO_V2.Shared.Models;
using static System.Net.WebRequestMethods;
using TODO_V2.Shared.Utils;

namespace TODO_V2.Server.Utils
{
    public sealed class EncryptionUtil
    {
        public readonly string _secret;

        public EncryptionUtil(IConfiguration configuration)
        {
            _secret = configuration["Encryption:Secret"];
        }

        public string Encrypt(string data)
        {
            byte[] clearBytes = Encoding.Unicode.GetBytes(data);

            using (var encryptor = Aes.Create())
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

            using (var encryptor = Aes.Create())
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

        public async Task<string?> BuildToken(User user, IConfiguration configuration)
        {
            SymmetricSecurityKey securityKey = new(Encoding.UTF8.GetBytes(configuration["JWT:Key"]));
            SigningCredentials credentials = new(securityKey, SecurityAlgorithms.HmacSha256);

            var principal = new ClaimsPrincipal(new ClaimsIdentity(null, "Basic"));
            var isAuthenticated = principal.Identity.IsAuthenticated;

            Claim[] claims =
           {
                new(ClaimTypes.NameIdentifier, user.Id.ToString()),
                new(ClaimTypes.Name, user.UserName),
                new(ClaimTypes.Role, user.UserType.ToString())
            };

            var expires = DateTime.UtcNow.AddHours(int.Parse(configuration["JWT:ExpirationHours"]));

            JwtSecurityToken token = new(
                issuer: configuration["JWT:Issuer"],
                audience: configuration["JWT:Audience"],
                claims: claims,
                expires: expires,
                signingCredentials: credentials
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
