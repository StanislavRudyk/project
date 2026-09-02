using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class Book
{
    public Guid Id { get; private set; }
    public BookTitle Title { get; private set; }
    public string? Description { get; private set; }
    public string? Author { get; private set; }
    public CoverKey? CoverKey { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }
    public Guid UploadedById { get; private set; }
    public ICollection<BookFile> Files { get; private set; } = [];

    private Book()
    {
    }

    public Book(
        BookTitle title,
        string? description,
        string? author,
        CoverKey? coverKey,
        Guid uploadedById)
    {
        Id = Guid.NewGuid();

        Title = title;
        Description = description;
        Author = author;
        CoverKey = coverKey;

        UploadedById = uploadedById;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void SetCoverKey(CoverKey coverKey)
    {
        CoverKey = coverKey;
    }

    public void AddFile(BookFile file)
    {
        if (file.BookId != Id)
            throw new InvalidOperationException(
                "Файл не принадлежит этой книге.");

        if (Files.Any(x => x.Format == file.Format))
            throw new InvalidOperationException(
                $"Формат {file.Format} уже добавлен.");

        Files.Add(file);
    }
}