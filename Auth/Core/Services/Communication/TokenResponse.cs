using Auth.Core.Security.Tokens;
using Microsoft.AspNetCore.Authentication.BearerToken;

namespace Auth.Core.Services.Communication
{
    public class TokenResponse : BaseResponse
    {
        public AccessToken Token { get; set; }

        public TokenResponse(bool success, string message, AccessToken token) : base(success, message)
        {
            Token = token;
        }
    }
}
