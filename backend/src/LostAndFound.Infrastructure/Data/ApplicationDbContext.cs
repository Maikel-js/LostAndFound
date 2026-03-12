using LostAndFound.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users => Set<User>();
    public DbSet<Item> Items => Set<Item>();
    public DbSet<Claim> Claims => Set<Claim>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>()
            .HasMany(u => u.ReportedItems)
            .WithOne(i => i.Reporter)
            .HasForeignKey(i => i.ReporterId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<User>()
            .HasMany(u => u.Claims)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Item>()
            .HasMany(i => i.Claims)
            .WithOne(c => c.Item)
            .HasForeignKey(c => c.ItemId)
            .OnDelete(DeleteBehavior.Cascade);
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();
            
        modelBuilder.Entity<User>()
            .HasIndex(u => u.EnrollmentNumber)
            .IsUnique();
    }
}
