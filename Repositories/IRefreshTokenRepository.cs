using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Repositories
{
    /// <summary>
    /// Interface for Refresh Token repository operations
    /// </summary>
    public interface IRefreshTokenRepository
    {
        /// <summary>
        /// Save a new refresh token to the database
        /// </summary>
        /// <param name="refreshToken">The refresh token to save</param>
        /// <returns>The saved refresh token with generated ID</returns>
        Task<RefreshToken> SaveRefreshTokenAsync(RefreshToken refreshToken);

        /// <summary>
        /// Get a refresh token by its token string
        /// </summary>
        /// <param name="token">The token string to search for</param>
        /// <returns>The refresh token if found, null otherwise</returns>
        Task<RefreshToken?> GetRefreshTokenByTokenStringAsync(string token);

        /// <summary>
        /// Get all refresh tokens for a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of all refresh tokens for the user</returns>
        Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(string userId);

        /// <summary>
        /// Get all active (non-revoked, non-expired) refresh tokens for a user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <returns>List of active refresh tokens</returns>
        Task<List<RefreshToken>> GetActiveRefreshTokensByUserIdAsync(string userId);

        /// <summary>
        /// Revoke a refresh token
        /// </summary>
        /// <param name="token">The token string to revoke</param>
        /// <param name="reason">Optional reason for revocation</param>
        /// <returns>True if revoked successfully, false if token not found</returns>
        Task<bool> RevokeRefreshTokenAsync(string token, string? reason = null);

        /// <summary>
        /// Replace a refresh token with a new one (used during token refresh)
        /// </summary>
        /// <param name="oldToken">The old token string to replace</param>
        /// <param name="newRefreshToken">The new refresh token</param>
        /// <returns>The new refresh token if successful, null if old token not found</returns>
        Task<RefreshToken?> ReplaceRefreshTokenAsync(string oldToken, RefreshToken newRefreshToken);

        /// <summary>
        /// Revoke all refresh tokens for a specific user
        /// </summary>
        /// <param name="userId">The user ID</param>
        /// <param name="reason">Optional reason for revocation</param>
        /// <returns>Number of tokens revoked</returns>
        Task<int> RevokeAllUserTokensAsync(string userId, string? reason = null);

        /// <summary>
        /// Delete expired tokens (cleanup operation)
        /// </summary>
        /// <returns>Number of tokens deleted</returns>
        Task<int> DeleteExpiredTokensAsync();
    }
}
