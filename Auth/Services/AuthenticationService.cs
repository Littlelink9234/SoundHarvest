using Auth.Core.Models;
using Auth.Core.Security.Hashing;
using Auth.Core.Security.Tokens;
using Auth.Core.Services;
using Auth.Core.Services.Communication;
using Microsoft.AspNetCore.Identity;
using System.Diagnostics;

namespace Auth.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly IUserService _userService;
        private readonly IPasswordHasher<User> _passwordHasher;
        private readonly ITokenHandler _tokenHandler;

        public AuthenticationService(IUserService userService, IPasswordHasher<User> passwordHasher, ITokenHandler tokenHandler)
        {
            _userService = userService;
            _passwordHasher = passwordHasher;
            _tokenHandler = tokenHandler;
        }

        public async Task<TokenResponse> CreateAccessTokenAsync(string email, string password)
        {
            var user = await _userService.FindByEmailAsync(email);

            if (user == null || _userService.ValidatePassword(user, password))
            {
                return new TokenResponse(false, "Invalid credentials", null);
            }

            var token = _tokenHandler.CreateAccessToken(user);
            return new TokenResponse(true, null, token);
        }

        public Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail)
        {
            throw new NotImplementedException();
        }

        public void RevokeRefreshToken(string refreshToken)
        {
            throw new NotImplementedException();
        }
    }
}
