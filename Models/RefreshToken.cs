using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace NearU_Backend_Revised.Models;

[Index("Token", Name = "IX_RefreshTokens_Token", IsUnique = true)]
[Index("UserId", Name = "IX_RefreshTokens_UserId")]
public partial class RefreshToken
{
    [Key]
    public int Id { get; set; }

    public string Token { get; set; } = null!;

    public DateTime ExpiryDate { get; set; }

    public DateTime CreatedDate { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedDate { get; set; }

    public string? ReplacedByToken { get; set; }

    public string? ReasonRevoked { get; set; }

    public string UserId { get; set; } = null!;

    [ForeignKey("UserId")]
    [InverseProperty("RefreshTokens")]
    public virtual User User { get; set; } = null!;

    [NotMapped]
    public bool IsExpired => DateTime.UtcNow >= ExpiryDate;

    [NotMapped]
    public bool IsRevoked => RevokedDate != null;

    [NotMapped]
    public bool IsActive => !IsRevoked && !IsExpired;
}
