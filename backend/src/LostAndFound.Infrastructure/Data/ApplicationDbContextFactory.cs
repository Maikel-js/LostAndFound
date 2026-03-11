using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.Extensions.Configuration;

namespace LostAndFound.Infrastructure.Data;

public class ApplicationDbContextFactory : IDesignTimeDbContextFactory<ApplicationDbContext>
{
    public ApplicationDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<ApplicationDbContext>();
        
        // This is only for design-time (migrations)
        // In a real scenario, you might want to read from appsettings.json
        optionsBuilder.UseNpgsql("Host=localhost;Database=lostandfound_db;Username=postgres;Password=password");

        return new ApplicationDbContext(optionsBuilder.Options);
    }
}
