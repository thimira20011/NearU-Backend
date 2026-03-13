namespace NearU_Backend_Revised.Configuration
{
    /// <summary>
    /// Configuration settings for JWT token generation and validation
    /// </summary>
    public class JwtSettings
    {
        /// <summary>
        /// Secret key for signing JWT tokens (must be at least 32 characters)
        /// </summary>
        public string SecretKey { get; set; } = string.Empty;

        /// <summary>
        /// Token issuer (your application name/URL)
        /// </summary>
        public string Issuer { get; set; } = string.Empty;

        /// <summary>
        /// Token audience (your client application)
        /// </summary>
        public string Audience { get; set; } = string.Empty;

        /// <summary>
        /// Access token expiry time in minutes (default: 15 minutes)
        /// </summary>
        public int AccessTokenExpiryInMinutes { get; set; } = 15;

        /// <summary>
        /// Refresh token expiry time in days (default: 7 days)
        /// </summary>
        public int RefreshTokenExpiryInDays { get; set; } = 7;
    }
}
