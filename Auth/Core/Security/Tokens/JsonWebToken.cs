namespace Auth.Core.Security.Tokens
{
    public abstract class JsonWebToken
    {
        public string Token { get; set; }
        public long Expiration { get; set; }

        public JsonWebToken(string token, long expiration)
        {
            if (string.IsNullOrWhiteSpace(token))
                throw new ArgumentNullException("Invalid Token");

            if (expiration <= 0)
                throw new ArgumentOutOfRangeException("Invalid expiration");

            Token = token;
            Expiration = expiration;
        }

        public bool IsExpired() => DateTime.UtcNow.Ticks > Expiration;
    }
}
