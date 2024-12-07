using Auth.Core.Models;
using Auth.Core.Security.Tokens;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.JsonWebTokens;
using Microsoft.IdentityModel.Tokens;
using System.Security.Claims;
using System.Text;
using JwtRegisteredClaimNames = Microsoft.IdentityModel.JsonWebTokens.JwtRegisteredClaimNames;

namespace Auth.Security.Tokens
{
    public class TokenHandler : ITokenHandler
    {
        private readonly ISet<RefreshToken> _refreshTokens = new HashSet<RefreshToken>();
        private readonly TokenOptions _tokenOptions;

        public TokenHandler(IOptions<TokenOptions> tokenOptionsSnapshot)
        {
            _tokenOptions = tokenOptionsSnapshot.Value;
        }

        public AccessToken CreateAccessToken(User user)
        {
            var refreshToken = BuildRefreshToken();
            var accessToken = BuildAccessToken(user, refreshToken);
            _refreshTokens.Add(refreshToken);

            return accessToken;
        }

        public void RevokeRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        public RefreshToken TakeRefreshToken(string token)
        {
            throw new NotImplementedException();
        }

        private RefreshToken BuildRefreshToken()
        {
            var refreshToken = new RefreshToken
                (
                    token: Guid.NewGuid().ToString(),
                    expiration: DateTime.UtcNow.AddDays(_tokenOptions.RefreshTokenExpirationDays).Ticks
                );

            return refreshToken;
        }

        private AccessToken BuildAccessToken(User user, RefreshToken refreshToken)
        {
            var accessTokenExpiration = DateTime.UtcNow.AddMinutes(_tokenOptions.AccessTokenExpirationMinutes);
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOptions.Secret));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256Signature);

            var claims = GetClaims(user);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Expires = accessTokenExpiration,
                Issuer = _tokenOptions.Issuer,
                Audience = _tokenOptions.Audience,
                SigningCredentials = creds
            };

            var tokenHandler = new JsonWebTokenHandler();
            var token = tokenHandler.CreateToken(tokenDescriptor);

            return new AccessToken(token, accessTokenExpiration.Ticks, refreshToken);
        }

        private IEnumerable<Claim> GetClaims(User user)
        {
            var claims = new List<Claim>
            {
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
                new Claim(JwtRegisteredClaimNames.Email, user.Email)
            };

            foreach (var userRole in user.UserRoles)
            {
                claims.Add(new Claim(ClaimTypes.Role, userRole.Role.Name));
            }

            return claims;
        }
    }
}
