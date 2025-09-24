using Onion.Assignment.Services.Greetings.Api.DTOs;
using Onion.Assignment.Services.Greetings.Domain.Entities;
using Onion.Assignment.Services.Greetings.Domain.Interfaces;
using Onion.Assignment.Services.Greetings.Domain.ValueObjects;

namespace Onion.Assignment.Services.Greetings.Domain.UseCases;

public class GreetingUseCases : IGreetingUseCases
{
    private readonly IGreetingRepository _greetingRepository;

    public GreetingUseCases(IGreetingRepository greetingRepository)
    {
        _greetingRepository = greetingRepository ?? throw new ArgumentNullException(nameof(greetingRepository));
    }

    public async Task<GreetingResponse> CreateGreetingAsync(CreateGreetingRequest request, CancellationToken cancellationToken = default)
    {
        // Domain validation happens in PersonName constructor
        var personName = new PersonName(request.Name);
        var greeting = new Greeting(personName);
        
        var savedGreeting = await _greetingRepository.SaveAsync(greeting, cancellationToken);
        
        return MapToResponse(savedGreeting);
    }

    public async Task<GreetingResponse?> GetGreetingByIdAsync(Guid id, CancellationToken cancellationToken = default)
    {
        var greeting = await _greetingRepository.GetByIdAsync(id, cancellationToken);
        
        return greeting != null ? MapToResponse(greeting) : null;
    }

    public async Task<IEnumerable<GreetingResponse>> GetAllGreetingsAsync(CancellationToken cancellationToken = default)
    {
        var greetings = await _greetingRepository.GetAllAsync(cancellationToken);
        
        return greetings.Select(MapToResponse);
    }

    public async Task<IEnumerable<GreetingResponse>> GetGreetingsByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        var greetings = await _greetingRepository.GetByNameAsync(name, cancellationToken);
        
        return greetings.Select(MapToResponse);
    }

    private static GreetingResponse MapToResponse(Greeting greeting)
    {
        return new GreetingResponse(
            greeting.Id,
            greeting.Name.Value,
            greeting.Message,
            greeting.CreatedAt
        );
    }
}