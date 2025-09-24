using Onion.Assignment.Services.Greetings;
using Onion.Assignment.Shared.Interfaces;
using System.Reflection;

namespace Onion.Assignment.Shared.DependencyInjection;

/// <summary>
/// Central dependency registration coordinator for the application.
/// Registers core services, third-party dependencies, and features.
/// </summary>
public static class DependencyRegistration
{
    /// <summary>
    /// Register all application dependencies including core services and features.
    /// </summary>
    /// <param name="builder">The WebApplication builder</param>
    public static void RegisterAllDependencies(this WebApplicationBuilder builder)
    {
    // Register core ASP.NET Core services
        RegisterCoreServices(builder.Services, builder.Configuration);
        
    // Register third-party dependencies
        RegisterThirdPartyServices(builder.Services, builder.Configuration);
        
    // Register all features
        RegisterFeatures(builder.Services, builder.Configuration);
    }

    /// <summary>
    /// Initialize all registered features.
    /// </summary>
    /// <param name="app">The built web application</param>
    public static async Task InitializeFeaturesAsync(this WebApplication app)
    {
        var features = GetAllFeatures();
        
        foreach (var feature in features)
        {
            try
            {
                app.Logger.LogInformation("Initializing feature: {FeatureName}", feature.FeatureName);
                await feature.InitializeAsync(app.Services, app.Environment);
                app.Logger.LogInformation("Successfully initialized feature: {FeatureName}", feature.FeatureName);
            }
            catch (Exception ex)
            {
                app.Logger.LogError(ex, "Failed to initialize feature: {FeatureName}", feature.FeatureName);
                throw; // Re-throw to fail fast on startup issues
            }
        }
    }

    private static void RegisterCoreServices(IServiceCollection services, IConfiguration configuration)
    {
        // ASP.NET Core MVC
        services.AddControllers();

    // Health checks
        services.AddHealthChecks();

        // Add memory caching
        services.AddMemoryCache();

        // Add HTTP client factory
        services.AddHttpClient();

        // Add configuration binding
        services.Configure<AppSettings>(configuration.GetSection("AppSettings"));
    }

    private static void RegisterThirdPartyServices(IServiceCollection services, IConfiguration configuration)
    {
    // OpenAPI/Swagger
        services.AddOpenApi();
        services.AddEndpointsApiExplorer();
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new() 
            { 
                Title = "Onion Assignment API - Feature Based Architecture", 
                Version = "v1",
                Description = "A REST API built with Feature-Based Architecture using Clean Architecture principles",
                Contact = new()
                {
                    Name = "Development Team",
                    Email = "dev@company.com"
                }
            });
            
            // Include XML comments when available
            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            if (File.Exists(xmlPath))
            {
                c.IncludeXmlComments(xmlPath);
            }

            // Add response type documentation
            c.DescribeAllParametersInCamelCase();
        });

        // CORS
        services.AddCors(options =>
        {
            options.AddPolicy("AllowedOrigins", policy =>
            {
                var allowedOrigins = configuration.GetSection("Cors:AllowedOrigins").Get<string[]>()
                                   ?? new[] { "http://localhost:3000", "https://localhost:3000" };
                
                policy.WithOrigins(allowedOrigins)
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });
    }

    private static void RegisterFeatures(IServiceCollection services, IConfiguration configuration)
    {
        var features = GetAllFeatures();
        
        foreach (var feature in features)
        {
            // Register databases first (other services may depend on them)
            feature.RegisterDatabases(services, configuration);
            
            // Then register other services
            feature.RegisterServices(services, configuration);
        }
    }

    private static List<IFeature> GetAllFeatures()
    {
        // Manually register known features (could be discovered via reflection)
        return new List<IFeature>
        {
            new GreetingsFeature()
            // Add more features here as needed
        };
    }
}

/// <summary>
/// Configuration model for application settings
/// </summary>
public class AppSettings
{
    public bool DetailedErrors { get; set; } = false;
    public string ApplicationName { get; set; } = "Onion Assignment API";
    public LoggingSettings Logging { get; set; } = new();
}

public class LoggingSettings
{
    public string LogLevel { get; set; } = "Information";
    public bool IncludeScopes { get; set; } = false;
}