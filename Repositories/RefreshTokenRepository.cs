using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Data;
using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Repositories
{
    /// <summary>
    /// Repository for managing refresh token database operations
    /// </summary>
    public class RefreshTokenRepository : IRefreshTokenRepository
    {
        private readonly ApplicationDbContext _context;

        public RefreshTokenRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Save a new refresh token to the database
        /// </summary>
        public async Task<RefreshToken> SaveRefreshTokenAsync(RefreshToken refreshToken)
        {
            _context.RefreshTokens.Add(refreshToken);
            await _context.SaveChangesAsync();
            return refreshToken;
        }

        /// <summary>
        /// Get a refresh token by its token string
        /// </summary>
        public async Task<RefreshToken?> GetRefreshTokenByTokenStringAsync(string token)
        {
            return await _context.RefreshTokens
                .Include(rt => rt.User) // Include user navigation property
                .FirstOrDefaultAsync(rt => rt.Token == token);
        }

        /// <summary>
        /// Get all refresh tokens for a specific user
        /// </summary>
        public async Task<List<RefreshToken>> GetRefreshTokensByUserIdAsync(string userId)
        {
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId)
                .OrderByDescending(rt => rt.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Get all active (non-revoked, non-expired) refresh tokens for a user
        /// </summary>
        public async Task<List<RefreshToken>> GetActiveRefreshTokensByUserIdAsync(string userId)
        {
            var now = DateTime.UtcNow;
            return await _context.RefreshTokens
                .Where(rt => rt.UserId == userId
                    && rt.RevokedDate == null
                    && rt.ExpiryDate > now)
                .OrderByDescending(rt => rt.CreatedDate)
                .ToListAsync();
        }

        /// <summary>
        /// Revoke a refresh token
        /// </summary>
        public async Task<bool> RevokeRefreshTokenAsync(string token, string? reason = null)
        {
            var refreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == token);

            if (refreshToken == null || refreshToken.IsRevoked)
                return false;

            refreshToken.RevokedDate = DateTime.UtcNow;
            refreshToken.ReasonRevoked = reason ?? "Revoked without reason";

            _context.RefreshTokens.Update(refreshToken);
            await _context.SaveChangesAsync();

            return true;
        }

        /// <summary>
        /// Replace a refresh token with a new one (used during token refresh)
        /// </summary>
        public async Task<RefreshToken?> ReplaceRefreshTokenAsync(string oldToken, RefreshToken newRefreshToken)
        {
            var oldRefreshToken = await _context.RefreshTokens
                .FirstOrDefaultAsync(rt => rt.Token == oldToken);

            if (oldRefreshToken == null)
                return null;

            // Revoke the old token
            oldRefreshToken.RevokedDate = DateTime.UtcNow;
            oldRefreshToken.ReplacedByToken = newRefreshToken.Token;
            oldRefreshToken.ReasonRevoked = "Replaced by new token";

            // Add the new token
            _context.RefreshTokens.Add(newRefreshToken);

            // Save both changes
            await _context.SaveChangesAsync();

            return newRefreshToken;
        }

        /// <summary>
        /// Revoke all refresh tokens for a specific user
        /// </summary>
        public async Task<int> RevokeAllUserTokensAsync(string userId, string? reason = null)
        {
            var activeTokens = await _context.RefreshTokens
                .Where(rt => rt.UserId == userId && rt.RevokedDate == null)
                .ToListAsync();

            if (!activeTokens.Any())
                return 0;

            var revokedCount = 0;
            var now = DateTime.UtcNow;
            var revokeReason = reason ?? "All tokens revoked";

            foreach (var token in activeTokens)
            {
                token.RevokedDate = now;
                token.ReasonRevoked = revokeReason;
                revokedCount++;
            }

            await _context.SaveChangesAsync();

            return revokedCount;
        }

        /// <summary>
        /// Delete expired tokens (cleanup operation)
        /// </summary>
        public async Task<int> DeleteExpiredTokensAsync()
        {
            var now = DateTime.UtcNow;

            // Delete tokens that are expired AND revoked (safe to delete)
            var expiredTokens = await _context.RefreshTokens
                .Where(rt => rt.ExpiryDate < now && rt.RevokedDate != null)
                .ToListAsync();

            if (!expiredTokens.Any())
                return 0;

            _context.RefreshTokens.RemoveRange(expiredTokens);
            await _context.SaveChangesAsync();

            return expiredTokens.Count;
        }
    }
}
