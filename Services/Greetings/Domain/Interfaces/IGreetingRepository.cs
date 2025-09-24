using Onion.Assignment.Services.Greetings.Domain.Entities;

namespace Onion.Assignment.Services.Greetings.Domain.Interfaces;

public interface IGreetingRepository
{
    Task<Greeting> SaveAsync(Greeting greeting, CancellationToken cancellationToken = default);
    Task<Greeting?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
    Task<IEnumerable<Greeting>> GetAllAsync(CancellationToken cancellationToken = default);
    Task<IEnumerable<Greeting>> GetByNameAsync(string name, CancellationToken cancellationToken = default);
}