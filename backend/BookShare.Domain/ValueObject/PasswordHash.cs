namespace BookShare.Domain.ValueObject;

public readonly record struct PasswordHash
{
    public string Value { get; }

    private PasswordHash(string value)
    {
        Value = value;
    }

    public static PasswordHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException();

        return new PasswordHash(value);
    }

    public static implicit operator string(PasswordHash hash)
        => hash.Value;
}