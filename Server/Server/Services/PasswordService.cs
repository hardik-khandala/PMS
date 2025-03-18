using System.Security.Cryptography;
using System.Text;

namespace Server.Services
{
    public class PasswordService
    {
        public string HashPassword(string passwordHash)
        {
            using var sha256 = SHA256.Create();
            return Convert.ToBase64String(sha256.ComputeHash(Encoding.UTF8.GetBytes(passwordHash)));
        }
        public bool VerifyPassword(string passwordHash1, string passwordHash2)
        {
            return HashPassword(passwordHash1) == passwordHash2;
        }
    }
}
