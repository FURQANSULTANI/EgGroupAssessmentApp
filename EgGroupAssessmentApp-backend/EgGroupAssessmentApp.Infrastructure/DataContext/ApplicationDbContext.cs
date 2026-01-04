using EgGroupAssessmentApp.Core.Models;
using Microsoft.EntityFrameworkCore;

namespace EgGroupAssessmentApp.Infrastructure.DataContext {

    public class ApplicationDbContext : DbContext {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options) {
        }

        public DbSet<User> Users { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder) {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id);
                entity.HasIndex(e => e.Username).IsUnique();
                entity.HasIndex(e => e.Email).IsUnique();

                entity.Property(e => e.Username)
                    .IsRequired()
                    .HasMaxLength(50);

                entity.Property(e => e.PasswordHash)
                    .IsRequired()
                    .HasMaxLength(255);

                entity.Property(e => e.Role)
                    .IsRequired()
                    .HasMaxLength(20);

                entity.Property(e => e.Email)
                    .IsRequired()
                    .HasMaxLength(100);
                entity.Property(e => e.CreatedAt)
                    .IsRequired();
                entity.Property(e => e.UpdatedAt);
            });

            modelBuilder.Entity<User>().HasData(
                new User {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = "$2a$12$5u6xsp6j/gVG7oXVJjAZ7eN78KzEn1Grr8XEE8J5UexGeMYhgfwva",
                    Role = "Admin",
                    Email = "admin@example.com",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                },
                new User {
                    Id = 2,
                    Username = "user",
                    PasswordHash = "$2a$12$WBDdaRbasuyCYekB5a7fbeKCHl/enPkLqvkpSifPDxR0gBFKKeXbG",
                    Role = "User",
                    Email = "user@example.com",
                    CreatedAt = new DateTime(2024, 1, 1, 0, 0, 0, DateTimeKind.Utc)
                }
            );
        }
    }
}