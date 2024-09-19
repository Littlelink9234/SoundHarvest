using Auth.Core.Security.Hashing;
using System.Security.Cryptography;

namespace Auth.Security.Hashing
{
    public class PasswordHasher : IPasswordHasher
    {
        public string HashPassword(string password)
        {
            byte[] hashedPassword = [];

            if (string.IsNullOrEmpty(password))
            {
                throw new ArgumentException("password", nameof(password));
            }

            using (var hmac = new HMACSHA512()) 
            {
                hashedPassword = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

            return Convert.ToBase64String(hashedPassword);
        }

        public bool PasswordMatches(string providedPassword, string passwordHash)
        {
            throw new NotImplementedException();
        }
    }
}
