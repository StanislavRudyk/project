namespace BookShare.Domain.ValueObject;

public readonly record struct RefreshTokenHash
{
    public string Value { get; }

    private RefreshTokenHash(string value)
    {
        Value = value;
    }

    public static RefreshTokenHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Refresh token hash cannot be empty.",
                nameof(value));

        return new RefreshTokenHash(value);
    }

    public static implicit operator string(RefreshTokenHash hash)
        => hash.Value;
}