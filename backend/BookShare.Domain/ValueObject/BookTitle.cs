namespace BookShare.Domain.ValueObject;

public readonly record struct BookTitle
{
    public string Value { get; }

    private BookTitle(string value)
    {
        Value = value;
    }

    public static BookTitle Create(string value)
    {
        if (string.IsNullOrWhiteSpace(value))
            throw new ArgumentException(
                "Название книги не может быть пустым.",
                nameof(value));

        value = value.Trim();

        if (value.Length > 300)
            throw new ArgumentException(
                "Название книги не может содержать больше 300 символов.",
                nameof(value));

        return new BookTitle(value);
    }

    public static implicit operator string(BookTitle title)
        => title.Value;

    public static explicit operator BookTitle(string value)
        => Create(value);
}