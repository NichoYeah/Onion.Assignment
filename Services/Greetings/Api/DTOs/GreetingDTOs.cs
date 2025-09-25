namespace Onion.Assignment.Services.Greetings.Api.DTOs;

public record CreateGreetingRequest(string Name, string? Message = null);

public record GreetingResponse(
    Guid Id,
    string Name,
    string Message,
    DateTime CreatedAt
);