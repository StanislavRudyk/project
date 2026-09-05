using System.Security.Claims;
using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class UploadBookHandler
{
    private readonly IBookRepository _books;
    private readonly IUserBookRepository _userBooks;
    private readonly IBookFileStorage _fileStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly BookUploadPreparationService _preparationService;

    public UploadBookHandler(
        IBookRepository books,
        IUserBookRepository userBooks,
        IBookFileStorage fileStorage,
        IUnitOfWork unitOfWork,
        BookUploadPreparationService preparationService)
    {
        _books = books;
        _userBooks = userBooks;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _preparationService = preparationService;
    }

    public async Task<UploadBookResult> HandleAsync(
        UploadBookRequest request,
        HttpContext context,
        CancellationToken cancellationToken = default)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            throw new UnauthorizedAccessException();

        using var preparation =
            await _preparationService.PrepareAsync(
                request,
                cancellationToken);

        var existingBook =
            await _books.FindByFileHashAsync(
                preparation.FileHash,
                cancellationToken);

        if (existingBook is not null)
        {
            return await HandleExistingBookAsync(
                existingBook,
                userId,
                cancellationToken);
        }

        var book = new Domain.Models.Book(
            BookTitle.Create(request.Title),
            request.Description,
            request.Author,
            null,
            userId);

        await using var bookStream =
            request.File.OpenReadStream();

        var fileKey =
            await _fileStorage.UploadAsync(
                bookStream,
                request.File.FileName,
                request.File.ContentType,
                cancellationToken);

        var bookFile = new BookFile(
            book.Id,
            preparation.Format,
            FileKey.Create(fileKey),
            preparation.FileHash,
            request.File.Length);

        book.AddFile(bookFile);

        preparation.CoverStream.Position = 0;

        var coverKey =
            await _fileStorage.UploadCoverAsync(
                preparation.CoverStream,
                book.Id,
                cancellationToken);

        book.SetCoverKey(
            CoverKey.Create(coverKey));

        var userBook = new UserBook(
            userId,
            book.Id);

        await _books.AddAsync(
            book,
            cancellationToken);

        await _userBooks.AddAsync(
            userBook,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new UploadBookResult(
            book.Id,
            BookCreated: true,
            UserBookCreated: true);
    }

    private async Task<UploadBookResult> HandleExistingBookAsync(
        Domain.Models.Book existingBook,
        Guid userId,
        CancellationToken cancellationToken)
    {
        var alreadyAdded =
            await _userBooks.ExistsAsync(
                userId,
                existingBook.Id,
                cancellationToken);

        if (alreadyAdded)
        {
            return new UploadBookResult(
                existingBook.Id,
                BookCreated: false,
                UserBookCreated: false);
        }

        var userBook = new UserBook(
            userId,
            existingBook.Id);

        await _userBooks.AddAsync(
            userBook,
            cancellationToken);

        await _unitOfWork.SaveChangesAsync(
            cancellationToken);

        return new UploadBookResult(
            existingBook.Id,
            BookCreated: false,
            UserBookCreated: true);
    }
}
