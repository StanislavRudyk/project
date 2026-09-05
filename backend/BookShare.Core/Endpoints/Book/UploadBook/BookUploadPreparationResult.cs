using BookShare.Domain.Enums;
using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class BookUploadPreparationResult : IDisposable
{
    public BookFormat Format { get; }
    public FileHash FileHash { get; }
    public Stream CoverStream { get; }

    public BookUploadPreparationResult(
        BookFormat format,
        FileHash fileHash,
        Stream coverStream)
    {
        Format = format;
        FileHash = fileHash;
        CoverStream = coverStream;
    }

    public void Dispose()
    {
        CoverStream.Dispose();
    }
}
