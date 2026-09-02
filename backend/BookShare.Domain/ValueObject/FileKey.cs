namespace BookShare.Domain.ValueObject;

public readonly record struct FileKey
{
    public string Value { get; }

    private FileKey(string value)
    {
        Value = value;
    }

    public static FileKey Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "File key не может быть пустым.",
                nameof(value));

        value = value.Trim();

        if (value.Length > 500)
            throw new ArgumentException(
                "File key не может содержать больше 500 символов.",
                nameof(value));

        return new FileKey(value);
    }

    public static implicit operator string(FileKey key)
        => key.Value;

    public static explicit operator FileKey(string value)
        => Create(value);
}