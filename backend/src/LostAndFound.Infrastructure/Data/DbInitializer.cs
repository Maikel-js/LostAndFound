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
        await context.Database.EnsureCreatedAsync();

        // 1. Seed erwin administrator
        User? erwin = await context.Users.FirstOrDefaultAsync(u => u.FirstName == "erwin");
        if (erwin == null)
        {
            erwin = new User
            {
                FirstName = "erwin",
                LastName = "Admin",
                Email = "erwin@gmail.com",
                EnrollmentNumber = "ERWIN-001",
                PasswordHash = HashPassword("1234"),
                Role = UserRole.Administrator
            };

            await context.Users.AddAsync(erwin);
        }
        else if (erwin.Email == "erwin@school.edu")
        {
            erwin.Email = "erwin@gmail.com";
            erwin.PasswordHash = HashPassword("1234"); // Ensure password is also correct
        }
        
        await context.SaveChangesAsync();

        // 2. Seed original admin if not exists
        if (!await context.Users.AnyAsync(u => u.Email == "admin@school.edu"))
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

        // 3. Seed 100 items if they don't exist
        if (await context.Items.CountAsync() < 100)
        {
            var random = new Random();
            var categories = Enum.GetValues<ItemCategory>();
            var statuses = Enum.GetValues<ItemStatus>();
            
            var itemNames = new[] { "Mochila", "Llaves", "Celular", "Cuaderno", "Termo", "Gafas", "Billetera", "Audífonos", "Chaqueta", "Gorra" };
            var locations = new[] { "Biblioteca", "Cafetería", "Laboratorio", "Gimnasio", "Auditorio", "Entrada Principal", "Estacionamiento", "Pasillo B", "Canchas", "Salón 101" };

            var items = new List<Item>();
            for (int i = 0; i < 100; i++)
            {
                items.Add(new Item
                {
                    Name = $"{itemNames[random.Next(itemNames.Length)]} {i + 1}",
                    Description = $"Descripción del objeto {i + 1}: Un objeto perdido en el campus.",
                    Category = categories[random.Next(categories.Length)],
                    LocationFound = locations[random.Next(locations.Length)],
                    DateFound = DateTime.UtcNow.AddDays(-random.Next(1, 30)),
                    Status = statuses[random.Next(statuses.Length)],
                    ReporterId = erwin.Id
                });
            }

            await context.Items.AddRangeAsync(items);
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
