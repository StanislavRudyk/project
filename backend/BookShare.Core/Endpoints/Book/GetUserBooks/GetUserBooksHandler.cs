using BookShare.Core.Endpoints.Book.GetBooks;
using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetUserBooks;

public sealed class GetUserBooksHandler
{
    private readonly IUserBookRepository _userBooks;

    public GetUserBooksHandler(IUserBookRepository userBooks)
    {
        _userBooks = userBooks;
    }

    public async Task<IReadOnlyList<GetBooksResponse>> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var books = await _userBooks.GetBooksByUserIdAsync(
            userId,
            cancellationToken);

        return books
            .Select(book => new GetBooksResponse(
                book.Id,
                book.Title,
                book.Description,
                book.Author,
                book.CoverKey,
                book.FileSize,
                book.CreatedAt))
            .ToList();
    }
}