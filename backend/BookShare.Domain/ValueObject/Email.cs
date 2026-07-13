using System.Text.RegularExpressions;

namespace BookShare.Domain.ValueObject;

public readonly record struct Email
{
    private static readonly Regex EmailRegex = new(
        @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
        RegexOptions.Compiled | RegexOptions.IgnoreCase);
    public string Value { get; }
    
    private Email(string value) => Value = value.ToLowerInvariant().Trim();

    public static Email Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
        {
            throw new ArgumentException("Email cannot be empty.", nameof(value));
        }

        if (!EmailRegex.IsMatch(value))
        {
            throw new ArgumentException("Invalid email format.", nameof(value));
        }

        return new Email(value);
    }
    
    public static implicit operator string(Email email) => email.Value;
    public static explicit operator Email(string email) => Create(email);
}