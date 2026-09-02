using BookShare.Domain.Abstractions;
using BookShare.Domain.Enums;

namespace BookShare.Core.Endpoints.Book.GetBookFile;

public sealed class GetBookFileHandler
{
    private readonly IUserBookRepository _userBooks;
    private readonly IBookFileStorage _fileStorage;

    public GetBookFileHandler(
        IUserBookRepository userBooks,
        IBookFileStorage fileStorage)
    {
        _userBooks = userBooks;
        _fileStorage = fileStorage;
    }

    public async Task<string?> HandleAsync(
        Guid userId,
        Guid bookId,
        BookFormat format,
        CancellationToken cancellationToken = default)
    {
        var book = await _userBooks.GetBookByUserIdAsync(
            userId,
            bookId,
            cancellationToken);

        if (book is null)
            return null;

        var bookFile = book.Files
            .FirstOrDefault(file => file.Format == format);

        if (bookFile is null)
            return null;

        return await _fileStorage.GetPresignedUrlAsync(
            bookFile.FileKey,
            cancellationToken: cancellationToken);
    }
}