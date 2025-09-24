using Onion.Assignment.Services.Greetings.Api.DTOs;

namespace Onion.Assignment.Services.Greetings.Domain.Interfaces;

public interface IGreetingUseCases
{
    Task<GreetingResponse> CreateGreetingAsync(CreateGreetingRequest request, CancellationToken cancellationToken = default);
    Task<GreetingResponse?> GetGreetingByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<GreetingResponse>> GetAllGreetingsAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<GreetingResponse>> GetGreetingsByNameAsync(string name, CancellationToken cancellationToken = default);
}