using System.Text.RegularExpressions;

namespace BookShare.Domain.ValueObject;

public readonly record struct FileHash
{
    private static readonly Regex HashRegex = new(
        "^[a-fA-F0-9]{64}$",
        RegexOptions.Compiled);

    public string Value { get; }

    private FileHash(string value)
    {
        Value = value.ToLowerInvariant();
    }

    public static FileHash Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "File hash не может быть пустым.",
                nameof(value));

        value = value.Trim();

        if (!HashRegex.IsMatch(value))
            throw new ArgumentException(
                "File hash должен быть корректным SHA-256 hash.",
                nameof(value));

        return new FileHash(value);
    }

    public static implicit operator string(FileHash hash)
        => hash.Value;

    public static explicit operator FileHash(string value)
        => Create(value);
}