namespace Auth.Core.Security.Tokens
{
    public class AccessToken : JsonWebToken
    {
        public RefreshToken RefreshToken { get; set; }

        public AccessToken(string token, long expiration, RefreshToken refreshToken) : base(token, expiration)
        {
            if (refreshToken == null)
            {
                throw new ArgumentNullException("Specify a valid refresh token");
            }

            RefreshToken = refreshToken;
        }
    }
}
