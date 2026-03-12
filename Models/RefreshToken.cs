using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace NearU_Backend_Revised.Models
{
    /// <summary>
    /// Represents a refresh token for long-lived authentication sessions.
    /// This is the record of every refresh token issued to users.
    /// </summary>
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        /// <summary>
        /// The actual refresh token string (should be unique and cryptographically secure)
        /// </summary>
        [Required]
        [MaxLength(500)]
        public string Token { get; set; } = string.Empty;

        /// <summary>
        /// When the refresh token expires
        /// </summary>
        [Required]
        public DateTime ExpiryDate { get; set; }

        /// <summary>
        /// When the refresh token was created
        /// </summary>
        [Required]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        /// <summary>
        /// When the refresh token was revoked (null if still active)
        /// </summary>
        public DateTime? RevokedDate { get; set; }

        /// <summary>
        /// Token that replaced this token when it was refreshed
        /// </summary>
        [MaxLength(500)]
        public string? ReplacedByToken { get; set; }

        /// <summary>
        /// Reason why the token was revoked (optional)
        /// </summary>
        [MaxLength(200)]
        public string? ReasonRevoked { get; set; }

        /// <summary>
        /// Foreign key to the User who owns this refresh token
        /// </summary>
        [Required]
        public string UserId { get; set; } = string.Empty;

        /// <summary>
        /// Navigation property to the User
        /// </summary>
        [ForeignKey(nameof(UserId))]
        public virtual User? User { get; set; }

        // Computed properties for easy checking

        /// <summary>
        /// Check if the token is expired
        /// </summary>
        [NotMapped]
        public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

        /// <summary>
        /// Check if the token has been revoked
        /// </summary>
        [NotMapped]
        public bool IsRevoked => RevokedDate != null;

        /// <summary>
        /// Check if the token is currently active (not expired and not revoked)
        /// </summary>
        [NotMapped]
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
