using EMS.API.Models;
using Microsoft.EntityFrameworkCore;
using BCrypt.Net;

namespace EMS.API.Data;

public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Employee> Employees { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<User> Users { get; set; } // For backward compatibility, can be removed later

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Employee>()
                .HasIndex(e => e.Email)
                .IsUnique();

            modelBuilder.Entity<AppUser>()
                .HasIndex(u => u.Username)
                .IsUnique();

            // Seed Users
            modelBuilder.Entity<AppUser>().HasData(
                new AppUser
                {
                    Id = 1,
                    Username = "admin",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("admin123"),
                    Role = "Admin"
                },
                new AppUser
                {
                    Id = 2,
                    Username = "viewer",
                    PasswordHash = BCrypt.Net.BCrypt.HashPassword("viewer123"),
                    Role = "Viewer"
                }
            );
        }
    }
