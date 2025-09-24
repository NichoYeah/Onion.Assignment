using Microsoft.EntityFrameworkCore;
using Onion.Assignment.Services.Greetings.Data.DataSources;
using Onion.Assignment.Services.Greetings.Data.Repositories;
using Onion.Assignment.Services.Greetings.Domain.Interfaces;
using Onion.Assignment.Services.Greetings.Domain.UseCases;
using Onion.Assignment.Shared.Interfaces;

namespace Onion.Assignment.Services.Greetings;

/// <summary>
/// Feature registration for the Greetings service
/// Handles all dependency registration and initialization for greeting-related functionality
/// </summary>
public class GreetingsFeature : IFeature
{
    public string FeatureName => "Greetings";

    public void RegisterServices(IServiceCollection services, IConfiguration configuration)
    {
        // Register Domain layer dependencies (Use Cases)
        services.AddScoped<IGreetingUseCases, GreetingUseCases>();
        
        // Register Data layer dependencies (Repositories)
        services.AddScoped<IGreetingRepository, SqliteGreetingRepository>();
        
        // Note: API layer (Controllers) are automatically registered by ASP.NET Core
        // when we call AddControllers() in the main registration
    }

    public void RegisterDatabases(IServiceCollection services, IConfiguration configuration)
    {
        // Register the GreetingDbContext with SQLite
        services.AddDbContext<GreetingDbContext>(options =>
        {
            var connectionString = configuration.GetConnectionString("GreetingsDatabase") 
                                ?? configuration.GetConnectionString("DefaultConnection")
                                ?? "Data Source=greetings.db";
            
            options.UseSqlite(connectionString);
            
            // Enable detailed errors in development
            if (configuration.GetValue<bool>("DetailedErrors"))
            {
                options.EnableDetailedErrors();
                options.EnableSensitiveDataLogging();
            }
        });
    }

    public async Task InitializeAsync(IServiceProvider serviceProvider, IWebHostEnvironment environment)
    {
        // Ensure database is created and up to date
        using var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetRequiredService<GreetingDbContext>();
        
        if (environment.IsDevelopment())
        {
            // In development, ensure database is created
            await dbContext.Database.EnsureCreatedAsync();
        }
        else
        {
            // In production, you might want to use migrations instead
            // await dbContext.Database.MigrateAsync();
            await dbContext.Database.EnsureCreatedAsync();
        }
        
        // Could add seed data here if needed
        // await SeedInitialDataAsync(dbContext);
    }

    private static async Task SeedInitialDataAsync(GreetingDbContext dbContext)
    {
        // Example: Add some initial data if the database is empty
        if (!await dbContext.Greetings.AnyAsync())
        {
            // Add seed data if needed
            // var initialGreeting = new GreetingDataModel { ... };
            // await dbContext.Greetings.AddAsync(initialGreeting);
            // await dbContext.SaveChangesAsync();
        }
    }
}