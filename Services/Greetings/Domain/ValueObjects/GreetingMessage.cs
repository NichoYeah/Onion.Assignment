namespace Onion.Assignment.Services.Greetings.Domain.ValueObjects;

public sealed record GreetingMessage
{
    public const int MaxLength = 200; // aligned with data model constraint
    public string Value { get; }

    public GreetingMessage(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        value = value.Trim();

        if (value.Length > MaxLength)
            throw new ArgumentException($"Message cannot exceed {MaxLength} characters.", nameof(value));

        Value = value;
    }

    public static GreetingMessage FromDefault(PersonName name) => new($"Hello, {name}!");

    public static implicit operator string(GreetingMessage message) => message.Value;
    public static implicit operator GreetingMessage(string value) => new(value);

    public override string ToString() => Value;
}
