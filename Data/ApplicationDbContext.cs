using Microsoft.EntityFrameworkCore;
using NearU_Backend_Revised.Models;

namespace NearU_Backend_Revised.Data
{
    /// <summary>
    /// Application database context for managing entities
    /// </summary>
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        // DbSets for entities
        public DbSet<Usee> Users { get; set; } = null!;
        public DbSet<RefreshToken> RefreshTokens { get; set; } = null!;

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Configure RefreshToken entity
            modelBuilder.Entity<RefreshToken>(entity =>
            {
                entity.HasKey(rt => rt.Id);

                entity.Property(rt => rt.Token)
                    .IsRequired()
                    .HasMaxLength(500);

                entity.Property(rt => rt.UserId)
                    .IsRequired();

                entity.Property(rt => rt.ExpiryDate)
                    .IsRequired();

                entity.Property(rt => rt.CreatedDate)
                    .IsRequired();

                entity.Property(rt => rt.ReplacedByToken)
                    .HasMaxLength(500);

                entity.Property(rt => rt.ReasonRevoked)
                    .HasMaxLength(200);

                // Create index on Token for faster lookups
                entity.HasIndex(rt => rt.Token)
                    .IsUnique();

                // Create index on UserId for faster user token queries
                entity.HasIndex(rt => rt.UserId);

                // Foreign key relationship with User
                entity.HasOne(rt => rt.User)
                    .WithMany(u => u.RefreshTokens)
                    .HasForeignKey(rt => rt.UserId)
                    .OnDelete(DeleteBehavior.Cascade);
            });

            // Configure User entity
            modelBuilder.Entity<Usee>(entity =>
            {
                entity.HasKey(u => u.Id);
                // Add other User configurations here as needed
            });
        }
    }
}
