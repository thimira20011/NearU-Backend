using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories;

namespace NearU_Backend_Revised.Examples
{
    /// <summary>
    /// Example usage of RefreshTokenRepository
    /// This file demonstrates how to use the repository methods
    /// </summary>
    public class RefreshTokenRepositoryUsageExample
    {
        private readonly IRefreshTokenRepository _tokenRepository;

        public RefreshTokenRepositoryUsageExample(IRefreshTokenRepository tokenRepository)
        {
            _tokenRepository = tokenRepository;
        }

        /// <summary>
        /// Example: Creating and saving a new refresh token
        /// </summary>
        public async Task<RefreshToken> CreateNewTokenExample(string userId)
        {
            var refreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(), // Use your token generation logic
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7), // 7 days expiry
                CreatedDate = DateTime.UtcNow
            };

            var savedToken = await _tokenRepository.SaveRefreshTokenAsync(refreshToken);
            return savedToken;
        }

        /// <summary>
        /// Example: Validating and getting a token
        /// </summary>
        public async Task<bool> ValidateTokenExample(string token)
        {
            var refreshToken = await _tokenRepository.GetRefreshTokenByTokenStringAsync(token);

            if (refreshToken == null)
                return false;

            // Check if token is active (not revoked and not expired)
            return refreshToken.IsActive;
        }

        /// <summary>
        /// Example: Refreshing a token (replacing old with new)
        /// </summary>
        public async Task<RefreshToken?> RefreshTokenExample(string oldToken, string userId)
        {
            // Create new token
            var newRefreshToken = new RefreshToken
            {
                Token = GenerateSecureToken(),
                UserId = userId,
                ExpiryDate = DateTime.UtcNow.AddDays(7),
                CreatedDate = DateTime.UtcNow
            };

            // Replace old token with new one
            var replacedToken = await _tokenRepository.ReplaceRefreshTokenAsync(oldToken, newRefreshToken);
            return replacedToken;
        }

        /// <summary>
        /// Example: Revoking a single token
        /// </summary>
        public async Task<bool> RevokeTokenExample(string token)
        {
            return await _tokenRepository.RevokeRefreshTokenAsync(token, "User logged out");
        }

        /// <summary>
        /// Example: Revoking all tokens for a user (e.g., on password change)
        /// </summary>
        public async Task<int> RevokeAllUserTokensExample(string userId)
        {
            var revokedCount = await _tokenRepository.RevokeAllUserTokensAsync(
                userId, 
                "Password changed - all sessions invalidated"
            );
            return revokedCount;
        }

        /// <summary>
        /// Example: Getting all active tokens for a user
        /// </summary>
        public async Task<List<RefreshToken>> GetUserActiveTokensExample(string userId)
        {
            var activeTokens = await _tokenRepository.GetActiveRefreshTokensByUserIdAsync(userId);
            return activeTokens;
        }

        /// <summary>
        /// Example: Cleanup expired tokens (run as scheduled job)
        /// </summary>
        public async Task<int> CleanupExpiredTokensExample()
        {
            var deletedCount = await _tokenRepository.DeleteExpiredTokensAsync();
            return deletedCount;
        }

        /// <summary>
        /// Helper method to generate secure token (placeholder)
        /// In production, use a cryptographically secure random generator
        /// </summary>
        private string GenerateSecureToken()
        {
            // This is a placeholder - use proper token generation
            // Example: Convert.ToBase64String(RandomNumberGenerator.GetBytes(64))
            return Guid.NewGuid().ToString("N");
        }
    }
}
