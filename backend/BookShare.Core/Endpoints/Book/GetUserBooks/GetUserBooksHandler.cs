using BookShare.Core.Endpoints.Book.GetBook;
using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetUserBooks;

public sealed class GetUserBooksHandler
{
    private readonly IUserBookRepository _userBooks;

    public GetUserBooksHandler(IUserBookRepository userBooks)
    {
        _userBooks = userBooks;
    }

    public async Task<IReadOnlyList<GetBookResponse>> HandleAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        var books = await _userBooks.GetBooksByUserIdAsync(
            userId,
            cancellationToken);

        return books
            .Select(book => new GetBookResponse(
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
                book.CreatedAt))
            .ToList();
    }
}