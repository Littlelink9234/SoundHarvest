using Auth.Core.Models;
using Auth.Core.Security.Hashing;
using Auth.Core.Security.Tokens;
using Microsoft.Extensions.Options;

namespace Auth.Security.Tokens
{
    public class TokenHandler : ITokenHandler
    {
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly TokenOptions _tokenOptions;
        private readonly SigningConfigurations _signingConfigurations;
        private readonly IPasswordHasher _passwordHasher;

        public TokenHandler(IOptions<TokenOptions> tokenOptionsSnapshot, SigningConfigurations signingConfigurations, IPasswordHasher passwordHasher)
        {
            _passwordHasher = passwordHasher;
            _tokenOptions = tokenOptionsSnapshot.Value;
            _signingConfigurations = signingConfigurations;
        }

        public AccessToken CreateAccessToken(User user)
        {
            throw new NotImplementedException();
        }

        public void RevokeRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public RefreshToken TakeRefreshToken(string token)
        {
            throw new NotImplementedException();
        }
    }
}
