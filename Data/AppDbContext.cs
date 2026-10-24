using Classwork_17_10_2026.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Classwork_17_10_2026.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<Product> Products { get; set; }
        public DbSet<User> Users { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Product>(entity =>
            {
                entity.HasKey(p => p.Id);
                entity.Property(p => p.Name).IsRequired().HasMaxLength(100);
                entity.Property(p => p.Price).IsRequired().HasColumnType("decimal(18,2)");
                entity.Property(p => p.Description).HasMaxLength(500);
                entity.Property(p => p.Quantity).IsRequired();
                entity.Property(p => p.CreatedDate).IsRequired();
                entity.Property(p => p.IsDeleted).IsRequired();
            });

            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(u => u.Id);
                entity.Property(u => u.Name).IsRequired().HasMaxLength(100);
                entity.Property(u => u.LastName).IsRequired().HasMaxLength(100);
                entity.Property(u => u.Email).IsRequired().HasMaxLength(255);
                entity.HasIndex(u => u.Email).IsUnique();
                entity.Property(u => u.Role).IsRequired().HasMaxLength(50);
                entity.Property(u => u.Age).IsRequired();
                entity.Property(u => u.CreatedAt).IsRequired();
                entity.Property(u => u.IsDeleted).IsRequired();
            });
        }
    }
}