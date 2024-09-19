using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace Auth.Security.Tokens
{
    public class SigningConfigurations
    {
        public SecurityKey SecurityKey { get; set; }
        public SigningCredentials SigningCredentials { get; set; }

        public SigningConfigurations(string key) 
        {
            var keyBytes = Encoding.ASCII.GetBytes(key);

            SecurityKey = new SymmetricSecurityKey(keyBytes);
            SigningCredentials = new SigningCredentials(SecurityKey, SecurityAlgorithms.HmacSha512Signature);
        }
    }
}
