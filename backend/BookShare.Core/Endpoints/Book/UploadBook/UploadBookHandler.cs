using System.Security.Claims;
using System.Security.Cryptography;
using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class UploadBookHandler
{
    private readonly IBookRepository _books;
    private readonly IUserBookRepository _userBooks;
    private readonly IBookFileStorage _fileStorage;
    private readonly IUnitOfWork _unitOfWork;

    public UploadBookHandler(
        IBookRepository books,
        IUserBookRepository userBooks,
        IBookFileStorage fileStorage,
        IUnitOfWork unitOfWork)
    {
        _books = books;
        _userBooks = userBooks;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(
        UploadBookRequest request,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException();

        if (request.File.Length == 0)
            throw new InvalidOperationException("File is empty.");

        if (request.File.ContentType != "application/pdf")
            throw new InvalidOperationException("Only PDF files are allowed.");

        await using var stream = request.File.OpenReadStream();

        // 1. Вычисляем хеш файла
        var hashBytes = await SHA256.HashDataAsync(
            stream,
            cancellationToken);

        var fileHash = Convert.ToHexString(hashBytes);

        // 2. Проверяем, есть ли уже такая книга
        var existingBook = await _books.FindByFileHashAsync(
            fileHash,
            cancellationToken);

        if (existingBook is not null)
        {
            var alreadyAdded = await _userBooks.ExistsAsync(
                userId,
                existingBook.Id,
                cancellationToken);

            if (!alreadyAdded)
            {
                var userBook = new UserBook(
                    userId,
                    existingBook.Id);

                await _userBooks.AddAsync(
                    userBook,
                    cancellationToken);

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);
            }

            return;
        }

        stream.Position = 0;

        var fileKey = await _fileStorage.UploadAsync(
            stream,
            request.File.FileName,
            request.File.ContentType,
            cancellationToken);

        var book = new Domain.Models.Book(
            request.Title,
            request.Description,
            request.Author,
            fileKey,
            fileHash,
            request.File.Length,
            userId);

        var userBookLink = new UserBook(
            userId,
            book.Id);

        await _books.AddAsync(
            book,
            cancellationToken);

        await _userBooks.AddAsync(
            userBookLink,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);
    }
}