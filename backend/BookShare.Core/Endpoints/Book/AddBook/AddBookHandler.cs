using System.Security.Claims;
using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;

namespace BookShare.Core.Endpoints.Book.AddBook;

public sealed class AddBookHandler
{
    private readonly IBookRepository _books;
    private readonly IUserBookRepository _userBooks;
    private readonly IUnitOfWork _unitOfWork;

    public AddBookHandler(
        IBookRepository books,
        IUserBookRepository userBooks,
        IUnitOfWork unitOfWork)
    {
        _books = books;
        _userBooks = userBooks;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        Guid bookId,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException();

        var book = await _books.GetByIdAsync(
            bookId,
            cancellationToken);

        if (book is null)
            throw new KeyNotFoundException("Book not found.");

        var alreadyAdded = await _userBooks.ExistsAsync(
            userId,
            bookId,
            cancellationToken);

        if (alreadyAdded)
            throw new InvalidOperationException(
                "Book is already in your library.");

        var userBook = new UserBook(
            userId,
            bookId);

        await _userBooks.AddAsync(
            userBook,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}