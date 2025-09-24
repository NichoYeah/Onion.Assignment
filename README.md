# Onion Assignment API - Feature Based Architecture

A modern .NET 9.0 REST API built with **Feature-Based Architecture** following Clean Architecture principles. This application demonstrates advanced architectural patterns including vertical slice architecture, domain-driven design, and dependency inversion.

## 🏗️ Architecture Overview

This application uses **Feature-Based Architecture** (also known as Vertical Slice Architecture) where each feature/service is self-contained with its own layers:

```text
Services/
└── [FeatureName]/
    ├── Data/           # Data access layer
    ├── Domain/         # Business logic layer  
    └── Api/            # Presentation layer
```

### Key Architectural Benefits

- ✅ **Feature Cohesion**: Everything related to a feature is in one place
- ✅ **Team Independence**: Teams can work on different features without conflicts  
- ✅ **Maintainability**: Changes are localized to specific features
- ✅ **Testability**: Easy to unit test each layer independently
- ✅ **Scalability**: Easy to add new features or extract to microservices

## 📁 Project Structure

```text
Onion.Assignment/
├── Services/                           # Features/Services
│   └── Greetings/                     # Greetings feature
│       ├── Data/                      # Data access layer
│       │   ├── Models/               # EF Core entities
│       │   ├── DataSources/          # DbContext & database access
│       │   └── Repositories/         # Repository implementations
│       ├── Domain/                    # Business logic layer
│       │   ├── Entities/             # Domain entities
│       │   ├── ValueObjects/         # Value objects with validation
│       │   ├── UseCases/             # Business use cases
│       │   └── Interfaces/           # Contracts/interfaces
│       ├── Api/                       # Presentation layer
│       │   ├── Controllers/          # REST API controllers
│       │   └── DTOs/                # Data transfer objects
│       └── GreetingsFeature.cs       # Feature dependency registration
├── Shared/                            # Shared/common code
│   ├── Interfaces/                   # Common interfaces
│   │   └── IFeature.cs              # Feature registration contract
│   └── DependencyInjection/         # Dependency registration
│       └── DependencyRegistration.cs # Central DI coordinator
├── Program.cs                         # Application entry point
├── appsettings.json                  # Application configuration
└── README.md                         # This file
```

## 🎯 Domain Model

### Greetings Feature

The Greetings feature demonstrates a simple greeting system with the following domain concepts:

- **PersonName** (Value Object): Validates and encapsulates a person's name
- **Greeting** (Entity): Represents a greeting with validation and business rules
- **IGreetingRepository** (Interface): Contract for data persistence
- **IGreetingUseCases** (Interface): Contract for business operations

## 🚀 API Endpoints

### Greetings Service

| Method | Endpoint | Description | Request Body | Response |
|--------|----------|-------------|--------------|----------|
| `POST` | `/api/greetings` | Create a new greeting | `{ "name": "string" }` | `GreetingResponse` |
| `GET` | `/api/greetings` | Get all greetings | - | `GreetingResponse[]` |
| `GET` | `/api/greetings/{id}` | Get greeting by ID | - | `GreetingResponse` |
| `GET` | `/api/greetings/by-name/{name}` | Get greetings by name | - | `GreetingResponse[]` |
| `GET` | `/hello?name={name}` | Legacy greeting endpoint | - | `string` |

### System Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| `GET` | `/` | Swagger UI documentation |
| `GET` | `/health` | Health check endpoint |
| `GET` | `/swagger/v1/swagger.json` | OpenAPI specification |

### Response Models

```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "name": "John Doe",
  "message": "Hello, John Doe!",
  "createdAt": "2025-09-24T10:30:00Z"
}
```

## 🛠️ Technologies Used

- **.NET 9.0** - Latest .NET framework
- **ASP.NET Core** - Web framework
- **Entity Framework Core** - ORM for database access
- **SQLite** - Lightweight database for development
- **Swagger/OpenAPI** - API documentation
- **Clean Architecture** - Architectural pattern
- **Feature-Based Architecture** - Organizational pattern

## 🏃‍♂️ Getting Started

### Prerequisites

- .NET 9.0 SDK
- Visual Studio 2022 or VS Code with C# extension

### Running the Application

1. **Clone the repository**

   ```bash
   git clone <repository-url>
   cd Onion.Assignment
   ```

2. **Restore dependencies**

   ```bash
   dotnet restore
   ```

3. **Build the application**

   ```bash
   dotnet build
   ```

4. **Run the application**

   ```bash
   dotnet run
   ```

5. **Access the API**
   - Swagger UI: <http://localhost:5242>
   - Health Check: <http://localhost:5242/health>
   - API Base URL: <http://localhost:5242/api>

## 🏛️ Architecture Principles

### 1. Feature-Based Organization

Each feature is self-contained with its own:

- **Data Layer**: Entity models, repositories, database context
- **Domain Layer**: Business entities, use cases, interfaces  
- **API Layer**: Controllers, DTOs, presentation logic

### 2. Dependency Inversion

- Inner layers (Domain) define interfaces
- Outer layers (Data, API) implement interfaces
- Dependencies flow inward toward business logic

### 3. Separation of Concerns

- **Domain Layer**: Pure business logic, no external dependencies
- **Data Layer**: Database access, entity mapping
- **API Layer**: HTTP concerns, serialization, validation

### 4. Testability

Each layer can be independently tested:

- **Unit Tests**: Domain entities and use cases
- **Integration Tests**: Repository implementations
- **API Tests**: Controller endpoints

## 🔧 Configuration

### Database Configuration

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Data Source=greetings.db",
    "GreetingsDatabase": "Data Source=greetings.db"
  }
}
```

### Application Settings

```json
{
  "AppSettings": {
    "ApplicationName": "Onion Assignment API",
    "DetailedErrors": false
  }
}
```

### CORS Configuration

```json
{
  "Cors": {
    "AllowedOrigins": [
      "http://localhost:3000",
      "https://localhost:3000"
    ]
  }
}
```

## 📝 Adding New Features

To add a new feature (e.g., "Users"):

1. **Create feature folder structure**:

   ```text
   Services/Users/
   ├── Data/
   ├── Domain/
   ├── Api/
   └── UsersFeature.cs
   ```

2. **Implement IFeature interface**:

   ```csharp
   public class UsersFeature : IFeature
   {
       public string FeatureName => "Users";
       // Implement required methods...
   }
   ```

3. **Register in DependencyRegistration**:

   ```csharp
   private static List<IFeature> GetAllFeatures()
   {
       return new List<IFeature>
       {
           new GreetingsFeature(),
           new UsersFeature() // Add here
       };
   }
   ```

## 📦 Deployment

### Production Considerations

1. **Database Migrations**: Replace `EnsureCreated()` with proper migrations
2. **Logging**: Configure structured logging (Serilog, etc.)
3. **Monitoring**: Add Application Insights or similar
4. **Security**: Implement authentication/authorization
5. **Performance**: Add caching, rate limiting

### Docker Deployment

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:9.0
WORKDIR /app
COPY . .
EXPOSE 80
ENTRYPOINT ["dotnet", "Onion.Assignment.dll"]
```

## 📚 Additional Resources

- [Clean Architecture by Robert Martin](https://blog.cleancoder.com/uncle-bob/2012/08/13/the-clean-architecture.html)
- [Vertical Slice Architecture](https://jimmybogard.com/vertical-slice-architecture/)
- [Feature Folders in ASP.NET Core](https://scottsauber.com/2016/04/25/feature-folder-structure-in-asp-net-core/)
- [.NET 9.0 Documentation](https://docs.microsoft.com/en-us/dotnet/)

---
