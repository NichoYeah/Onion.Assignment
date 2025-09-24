namespace Onion.Assignment.Services.Greetings.Domain.ValueObjects;

public record PersonName
{
    public string Value { get; init; }

    public PersonName(string value)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(value, nameof(value));
        
        if (value.Length > 100)
            throw new ArgumentException("Name cannot exceed 100 characters.", nameof(value));
            
        Value = value.Trim();
    }

    public static implicit operator string(PersonName name) => name.Value;
    public static implicit operator PersonName(string value) => new(value);

    public override string ToString() => Value;
}