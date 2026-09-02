using BookShare.Core.Endpoints.Book.GetBooks;
using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetBook;

public sealed class GetBookHandler
{
    private readonly IBookRepository _books;

    public GetBookHandler(IBookRepository books)
    {
        _books = books;
    }

    public async Task<GetBooksResponse?> HandleAsync(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var book = await _books.GetByIdAsync(
            bookId,
            cancellationToken);

        if (book is null)
            return null;

        return new GetBooksResponse(
            book.Id,
            book.Title,
            book.Description,
            book.Author,
            book.CoverKey,
            book.FileSize,
            book.CreatedAt);
    }
}