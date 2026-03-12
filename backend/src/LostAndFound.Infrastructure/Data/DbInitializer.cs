using System.Security.Cryptography;
using System.Text;
using LostAndFound.Domain.Entities;
using LostAndFound.Domain.Enums;
using Microsoft.EntityFrameworkCore;

namespace LostAndFound.Infrastructure.Data;

public static class DbInitializer
{
    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await context.Database.MigrateAsync();

        if (!await context.Users.AnyAsync(u => u.Role == UserRole.Administrator))
        {
            var admin = new User
            {
                FirstName = "Admin",
                LastName = "Principal",
                Email = "admin@school.edu",
                EnrollmentNumber = "ADMIN-001",
                PasswordHash = HashPassword("Admin123!"),
                Role = UserRole.Administrator
            };

            await context.Users.AddAsync(admin);
            await context.SaveChangesAsync();
        }
    }

    private static string HashPassword(string password)
    {
        using var sha256 = SHA256.Create();
        var hashedBytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(password));
        return BitConverter.ToString(hashedBytes).Replace("-", "").ToLower();
    }
}
