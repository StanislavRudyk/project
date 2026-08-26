
namespace BookShare.Domain.Models;

public sealed class Book
{
    public Guid Id { get; private set; }

    public string Title { get; private set; } = null!;

    public string? Description { get; private set; }

    public string? Author { get; private set; }

    public string? CoverKey { get; private set; }

    public string FileKey { get; private set; } = null!;

    public string FileHash { get; private set; } = null!;

    public long FileSize { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public Guid UploadedById { get; private set; }

    private Book()
    {
    }

    public Book(
        string title,
        string? description,
        string? author,
        string fileKey,
        string fileHash,
        long fileSize,
        Guid uploadedById)
    {
        Id = Guid.NewGuid();

        Title = title;
        Description = description;
        Author = author;

        FileKey = fileKey;
        FileHash = fileHash;
        FileSize = fileSize;

        UploadedById = uploadedById;
        CreatedAt = DateTimeOffset.UtcNow;
    }
}