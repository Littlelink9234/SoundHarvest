using Auth.Core.Services.Communication;

namespace Auth.Core.Services
{
    public interface IAuthenticationService
    {
        Task<TokenResponse> CreateAccessTokenAsync(string email, string password);

        Task<TokenResponse> RefreshTokenAsync(string refreshToken, string userEmail);
        
        void RevokeRefreshToken(string refreshToken);
    }
}
