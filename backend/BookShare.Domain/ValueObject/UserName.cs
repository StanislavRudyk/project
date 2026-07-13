namespace BookShare.Domain.ValueObject;

public readonly record struct UserName
{
    public string Value { get; }

    private UserName(string value) => Value = value;

    public static UserName Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException("Имя пользователя не может быть пустым.");
            
        if (value.Length < 3 || value.Length > 20)
            throw new ArgumentException("Имя должно быть от 3 до 20 символов.");

        if (!value.All(char.IsLetterOrDigit))
            throw new ArgumentException("Имя может содержать только буквы и цифры.");

        return new UserName(value);
    }

    public static implicit operator string(UserName name) => name.Value;
    public static explicit operator UserName(string value) => Create(value);
}