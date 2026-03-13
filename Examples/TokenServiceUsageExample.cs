using NearU_Backend_Revised.Models;
using NearU_Backend_Revised.Repositories;
using NearU_Backend_Revised.Services;

namespace NearU_Backend_Revised.Examples
{
    /// <summary>
    /// Example usage of TokenService
    /// This demonstrates how to generate and validate tokens
    /// </summary>
    public class TokenServiceUsageExample
    {
        private readonly ITokenService _tokenService;

        public TokenServiceUsageExample(ITokenService tokenService)
        {
            _tokenService = tokenService;
        }

        /// <summary>
        /// Example 1: Generate access token for a student
        /// </summary>
        public string GenerateStudentAccessToken()
        {
            var student = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "john_doe",
                Email = "john.doe@student.edu",
                Role = UserRoles.Student,
                PasswordHash = "hashed_password",
                IsActive = true
            };

            var accessToken = _tokenService.GenerateAccessToken(student);

            // Access token is a JWT string like:
            // "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9..."
            return accessToken;
        }

        /// <summary>
        /// Example 2: Generate access token for a rider
        /// </summary>
        public string GenerateRiderAccessToken()
        {
            var rider = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "delivery_rider",
                Email = "rider@nearu.com",
                Role = UserRoles.Rider,
                PasswordHash = "hashed_password",
                IsActive = true
            };

            return _tokenService.GenerateAccessToken(rider);
        }

        /// <summary>
        /// Example 3: Generate access token for a business
        /// </summary>
        public string GenerateBusinessAccessToken()
        {
            var business = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "local_restaurant",
                Email = "restaurant@business.com",
                Role = UserRoles.Business,
                PasswordHash = "hashed_password",
                IsActive = true
            };

            return _tokenService.GenerateAccessToken(business);
        }

        /// <summary>
        /// Example 4: Generate access token for an admin
        /// </summary>
        public string GenerateAdminAccessToken()
        {
            var admin = new User
            {
                Id = Guid.NewGuid().ToString(),
                Username = "admin_user",
                Email = "admin@nearu.com",
                Role = UserRoles.Admin,
                PasswordHash = "hashed_password",
                IsActive = true
            };

            return _tokenService.GenerateAccessToken(admin);
        }

        /// <summary>
        /// Example 5: Generate a refresh token
        /// </summary>
        public RefreshToken GenerateRefreshTokenExample(string userId)
        {
            var refreshToken = _tokenService.GenerateRefreshToken(userId);

            // Refresh token properties:
            // - Token: "base64 encoded secure random string"
            // - UserId: user's ID
            // - CreatedDate: current UTC time
            // - ExpiryDate: 7 days from now (configurable)

            return refreshToken;
        }

        /// <summary>
        /// Example 6: Complete login flow (access + refresh tokens)
        /// </summary>
        public (string accessToken, RefreshToken refreshToken) CompleteLoginFlow(User user)
        {
            // Generate both tokens
            var accessToken = _tokenService.GenerateAccessToken(user);
            var refreshToken = _tokenService.GenerateRefreshToken(user.Id);

            // In a real application, you would save the refresh token to the database here
            // using the RefreshTokenRepository

            return (accessToken, refreshToken);
        }

        /// <summary>
        /// Example 7: Validate an access token
        /// </summary>
        public bool ValidateAccessTokenExample(string token)
        {
            var userId = _tokenService.ValidateAccessToken(token);

            if (userId != null)
            {
                Console.WriteLine($"Token is valid for user: {userId}");
                return true;
            }
            else
            {
                Console.WriteLine("Token is invalid or expired");
                return false;
            }
        }

        /// <summary>
        /// Example 8: Generate tokens for all role types
        /// </summary>
        public Dictionary<string, string> GenerateTokensForAllRoles()
        {
            var tokens = new Dictionary<string, string>();

            foreach (var role in UserRoles.AllRoles)
            {
                var user = new User
                {
                    Id = Guid.NewGuid().ToString(),
                    Username = $"{role.ToLower()}_user",
                    Email = $"{role.ToLower()}@nearu.com",
                    Role = role,
                    PasswordHash = "hashed_password",
                    IsActive = true
                };

                tokens[role] = _tokenService.GenerateAccessToken(user);
            }

            return tokens;
        }

        /// <summary>
        /// Example 9: Token refresh scenario
        /// </summary>
        public async Task<(string newAccessToken, RefreshToken newRefreshToken)> RefreshTokenScenario(
            string oldAccessToken, 
            string oldRefreshToken,
            IRefreshTokenRepository tokenRepository)
        {
            // Step 1: Validate the old access token (even if expired, we can extract user ID)
            var userId = _tokenService.ValidateAccessToken(oldAccessToken);

            // Step 2: Validate the refresh token from database
            var storedRefreshToken = await tokenRepository.GetRefreshTokenByTokenStringAsync(oldRefreshToken);

            if (storedRefreshToken == null || !storedRefreshToken.IsActive)
            {
                throw new UnauthorizedAccessException("Invalid refresh token");
            }

            // Step 3: Generate new tokens
            var user = storedRefreshToken.User; // Assuming navigation property is loaded
            if (user == null)
            {
                throw new InvalidOperationException("User not found");
            }

            var newAccessToken = _tokenService.GenerateAccessToken(user);
            var newRefreshToken = _tokenService.GenerateRefreshToken(user.Id);

            // Step 4: Replace old refresh token with new one in database
            await tokenRepository.ReplaceRefreshTokenAsync(oldRefreshToken, newRefreshToken);

            return (newAccessToken, newRefreshToken);
        }
    }
}
