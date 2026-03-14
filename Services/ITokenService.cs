using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Services
{
    /// <summary>
    /// Interface for token generation services
    /// </summary>
    public interface ITokenService
    {
        /// <summary>
        /// Generate a JWT access token for a user
        /// </summary>
        /// <param name="user">The user to generate token for</param>
        /// <returns>JWT token string</returns>
        string GenerateAccessToken(User user);

        /// <summary>
        /// Generate a secure refresh token
        /// </summary>
        /// <returns>Refresh token entity</returns>
        RefreshToken GenerateRefreshToken(string userId);

        /// <summary>
        /// Validate and extract claims from a JWT token
        /// </summary>
        /// <param name="token">JWT token to validate</param>
        /// <returns>User ID if valid, null otherwise</returns>
        string? ValidateAccessToken(string token);

        /// <summary>
        /// Validate a refresh token (check if exists, not expired, not revoked)
        /// </summary>
        /// <param name="token">Refresh token string to validate</param>
        /// <returns>RefreshToken entity if valid, null otherwise</returns>
        Task<RefreshToken?> ValidateRefreshToken(string token);

        /// <summary>
        /// Rotate a refresh token (revoke old token and generate new one)
        /// </summary>
        /// <param name="oldToken">Old refresh token string to rotate</param>
        /// <returns>New RefreshToken entity if successful, null otherwise</returns>
        Task<RefreshToken?> RotateRefreshToken(string oldToken);
    }
}
