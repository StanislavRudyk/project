using System.Security.Claims;
using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Book.DeleteBook;

public sealed class DeleteBookHandler
{
    private readonly IUserBookRepository _userBooks;
    private readonly IUnitOfWork _unitOfWork;

    public DeleteBookHandler(
        IUserBookRepository userBooks,
        IUnitOfWork unitOfWork)
    {
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

        var exists = await _userBooks.ExistsAsync(
            userId,
            bookId,
            cancellationToken);

        if (!exists)
            return;

        await _userBooks.DeleteAsync(
            userId,
            bookId,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}