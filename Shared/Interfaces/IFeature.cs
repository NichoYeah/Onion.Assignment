using Microsoft.EntityFrameworkCore;

namespace Onion.Assignment.Shared.Interfaces;

/// <summary>
/// Interface that each feature/service must implement for dependency registration
/// </summary>
public interface IFeature
{
    /// <summary>
    /// The name of the feature for logging and identification purposes
    /// </summary>
    string FeatureName { get; }

    /// <summary>
    /// Register all dependencies specific to this feature
    /// </summary>
    /// <param name="services">The service collection to register dependencies with</param>
    /// <param name="configuration">Application configuration for connection strings, settings, etc.</param>
    void RegisterServices(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Configure any database contexts specific to this feature
    /// </summary>
    /// <param name="services">The service collection to register DbContexts with</param>
    /// <param name="configuration">Application configuration for connection strings</param>
    void RegisterDatabases(IServiceCollection services, IConfiguration configuration);

    /// <summary>
    /// Perform any initialization that needs to happen after the container is built
    /// </summary>
    /// <param name="serviceProvider">The built service provider</param>
    /// <param name="environment">The hosting environment</param>
    Task InitializeAsync(IServiceProvider serviceProvider, IWebHostEnvironment environment);
}