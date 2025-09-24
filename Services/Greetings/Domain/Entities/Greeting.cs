using Onion.Assignment.Services.Greetings.Domain.ValueObjects;

namespace Onion.Assignment.Services.Greetings.Domain.Entities;

public class Greeting
{
    public Guid Id { get; private set; }
    public PersonName Name { get; private set; }
    public string Message { get; private set; }
    public DateTime CreatedAt { get; private set; }

    public Greeting(PersonName name)
    {
        Id = Guid.NewGuid();
        Name = name ?? throw new ArgumentNullException(nameof(name));
        Message = $"Hello, {name}!";
        CreatedAt = DateTime.UtcNow;
    }

    // For reconstruction from persistence
    private Greeting(Guid id, PersonName name, string message, DateTime createdAt)
    {
        Id = id;
        Name = name;
        Message = message;
        CreatedAt = createdAt;
    }

    public static Greeting Reconstruct(Guid id, PersonName name, string message, DateTime createdAt)
        => new(id, name, message, createdAt);
}