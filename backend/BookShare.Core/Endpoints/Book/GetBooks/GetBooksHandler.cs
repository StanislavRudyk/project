using System.Security.Claims;
using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.GetBooks;

public sealed class GetBooksHandler
{
    private readonly IUserBookRepository _userBooks;

    public GetBooksHandler(IUserBookRepository userBooks)
    {
        _userBooks = userBooks;
    }

    public async Task<IReadOnlyList<GetBooksResponse>> HandleAsync(
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException();

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