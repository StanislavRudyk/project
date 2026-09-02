namespace BookShare.Domain.ValueObject;

public readonly record struct CoverKey
{
    public string Value { get; }

    private CoverKey(string value)
    {
        Value = value;
    }

    public static CoverKey Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Cover key не может быть пустым.",
                nameof(value));

        value = value.Trim();

        if (value.Length > 500)
            throw new ArgumentException(
                "Cover key не может содержать больше 500 символов.",
                nameof(value));

        return new CoverKey(value);
    }

    public static implicit operator string(CoverKey key)
        => key.Value;

    public static explicit operator CoverKey(string value)
        => Create(value);
}