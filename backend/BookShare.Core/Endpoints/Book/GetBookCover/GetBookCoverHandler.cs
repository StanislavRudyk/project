using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetBookCover;

public sealed class GetBookCoverHandler
{
    private readonly IBookRepository _books;
    private readonly IBookFileStorage _fileStorage;

    public GetBookCoverHandler(
        IBookRepository books,
        IBookFileStorage fileStorage)
    {
        _books = books;
        _fileStorage = fileStorage;
    }

    public async Task<Stream?> HandleAsync(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var book = await _books.GetByIdAsync(
            bookId,
            cancellationToken);

        if (book is null || string.IsNullOrWhiteSpace(book.CoverKey))
            return null;

        return await _fileStorage.GetAsync(
            book.CoverKey,
            cancellationToken);
    }
}