namespace Onion.Assignment.Services.Greetings.Api.DTOs;

public record CreateGreetingRequest(string Name);

public record GreetingResponse(
    Guid Id,
    string Name,
    string Message,
    DateTime CreatedAt
);