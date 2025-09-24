using Onion.Assignment.Shared.DependencyInjection;

var builder = WebApplication.CreateBuilder(args);

// Register all dependencies using the centralized registration system
// This includes core services, third-party dependencies, and all features
builder.RegisterAllDependencies();

var app = builder.Build();

// Configure the HTTP request pipeline
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.SwaggerEndpoint("/swagger/v1/swagger.json", "Onion Assignment API - Feature Based v1");
        c.RoutePrefix = string.Empty; // Serve Swagger UI at the app's root
        c.DocExpansion(Swashbuckle.AspNetCore.SwaggerUI.DocExpansion.List);
        c.DefaultModelsExpandDepth(-1); // Hide schemas section
    });
}

// Security headers and HTTPS redirection
app.UseHttpsRedirection();

// Enable CORS (configured in dependency registration)
app.UseCors("AllowedOrigins");

// Add health check endpoint
app.MapHealthChecks("/health");

// Map API controllers
app.MapControllers();

// Initialize all features (databases, seed data, etc.)
await app.InitializeFeaturesAsync();

app.Logger.LogInformation("🚀 Application started successfully!");
app.Logger.LogInformation("📚 Swagger UI available at: {BaseUrl}", app.Environment.IsDevelopment() ? "http://localhost:5242" : "Application URL");
app.Logger.LogInformation("🔍 Health check available at: {BaseUrl}/health", app.Environment.IsDevelopment() ? "http://localhost:5242" : "Application URL");

app.Run();