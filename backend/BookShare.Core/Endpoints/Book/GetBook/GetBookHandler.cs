using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetBook;

public sealed class GetBookHandler
{
    private readonly IBookRepository _books;

    public GetBookHandler(IBookRepository books)
    {
        _books = books;
    }

    public async Task<GetBookResponse?> HandleAsync(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var book = await _books.GetByIdAsync(
            bookId,
            cancellationToken);

        if (book is null)
            return null;

        return new GetBookResponse(
            book.Id,
            book.Title,
            book.Description,
            book.Author,
            book.CoverKey,
            book.Files
                .Select(file => new BookFileResponse(
                    file.Id,
                    file.Format,
                    file.FileSize))
                .ToList(),
            book.CreatedAt);
    }
}