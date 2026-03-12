using System.ComponentModel.DataAnnotations;

namespace NearU_Backend_Revised.Models
{
    /// <summary>
    /// User entity representing application users
    /// </summary>
    public class User
    {
        [Key]
        public string Id { get; set; } = Guid.NewGuid().ToString();

        [Required]
        [MaxLength(100)]
        public string Username { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [MaxLength(255)]
        public string Email { get; set; } = string.Empty;

        [Required]
        public string PasswordHash { get; set; } = string.Empty;

        public string Role { get; set; } = "Student";

        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

        public DateTime? LastLoginDate { get; set; }

        public bool IsActive { get; set; } = true;

        // Navigation property for refresh tokens
        public virtual ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
