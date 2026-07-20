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
        return new RefreshTokenHash(value);
    }
}