using BookShare.Domain.Enums;
using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class BookFile
{
    public Guid Id { get; private set; }

    public Guid BookId { get; private set; }

    public BookFormat Format { get; private set; }

    public FileKey FileKey { get; private set; }

    public FileHash FileHash { get; private set; }

    public long FileSize { get; private set; }

    public DateTimeOffset CreatedAt { get; private set; }

    public Book Book { get; private set; } = null!;

    private BookFile()
    {
    }

    public BookFile(
        Guid bookId,
        BookFormat format,
        FileKey fileKey,
        FileHash fileHash,
        long fileSize)
    {
        if (fileSize <= 0)
            throw new ArgumentOutOfRangeException(
                nameof(fileSize),
                "Размер файла должен быть больше нуля.");

        Id = Guid.NewGuid();

        BookId = bookId;
        Format = format;
        FileKey = fileKey;
        FileHash = fileHash;
        FileSize = fileSize;

        CreatedAt = DateTimeOffset.UtcNow;
    }
}