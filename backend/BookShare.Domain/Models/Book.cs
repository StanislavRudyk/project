using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class Book
{
    public Guid Id { get; private set; }

    public BookTitle Title { get; private set; }

    public string? Description { get; private set; }

    public string? Author { get; private set; }

    public CoverKey? CoverKey { get; private set; }

    public FileKey FileKey { get; private set; }

    public FileHash FileHash { get; private set; }

    public long FileSize { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public Guid UploadedById { get; private set; }

    private Book()
    {
    }

    public Book(
        BookTitle title,
        string? description,
        string? author,
        FileKey fileKey,
        CoverKey? coverKey,
        FileHash fileHash,
        long fileSize,
        Guid uploadedById)
    {
        if (fileSize <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(fileSize),
                "Размер файла должен быть больше нуля.");

        Id = Guid.NewGuid();

        Title = title;
        Description = description;
        Author = author;

        FileKey = fileKey;
        CoverKey = coverKey;

        FileHash = fileHash;
        FileSize = fileSize;

        UploadedById = uploadedById;
        CreatedAt = DateTimeOffset.UtcNow;
    }

    public void SetCoverKey(CoverKey coverKey)
    {
        CoverKey = coverKey;
    }
}