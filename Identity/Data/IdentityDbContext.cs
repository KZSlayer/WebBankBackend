using Identity.Models;
using Microsoft.EntityFrameworkCore;
namespace Identity.Data
{
    public class IdentityDbContext : DbContext
    {
        public DbSet<User> users { get; set; }
        public DbSet<RefreshToken> refresh_tokens { get; set; }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=WebBank_IdentityDB;Username=postgres;Password=nvidia123");
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<User>()
                .HasIndex(u => u.Email)
                .IsUnique();

            modelBuilder.Entity<User>()
                .HasIndex(u => u.PhoneNumber)
                .IsUnique();
        }
    }
}
