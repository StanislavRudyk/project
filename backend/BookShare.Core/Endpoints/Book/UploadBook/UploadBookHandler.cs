using System.Security.Claims;
using System.Security.Cryptography;
using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using BookShare.Infrastructure.Postgres.Pdf;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class UploadBookHandler
{
    private readonly IBookRepository _books;
    private readonly IUserBookRepository _userBooks;
    private readonly IBookFileStorage _fileStorage;
    private readonly IUnitOfWork _unitOfWork;
    private readonly PdfCoverGenerator _coverGenerator;

    public UploadBookHandler(
        IBookRepository books,
        IUserBookRepository userBooks,
        IBookFileStorage fileStorage,
        IUnitOfWork unitOfWork,
        PdfCoverGenerator coverGenerator)
    {
        _books = books;
        _userBooks = userBooks;
        _fileStorage = fileStorage;
        _unitOfWork = unitOfWork;
        _coverGenerator = coverGenerator;
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

        if (request.File is null || request.File.Length == 0)
            throw new InvalidOperationException("File is empty.");

        if (request.File.ContentType != "application/pdf")
            throw new InvalidOperationException(
                "Only PDF files are allowed.");

        await using var stream = request.File.OpenReadStream();

        var hashBytes = await SHA256.HashDataAsync(
            stream,
            cancellationToken);

        var fileHash = FileHash.Create(
            Convert.ToHexString(hashBytes));

        var existingBook = await _books.FindByFileHashAsync(
            fileHash,
            cancellationToken);

        if (existingBook is not null)
        {
            if (existingBook.CoverKey is null)
            {
                stream.Position = 0;

                await using var generatedCover =
                    _coverGenerator.Generate(stream);

                var generatedCoverKey =
                    await _fileStorage.UploadCoverAsync(
                        generatedCover,
                        existingBook.Id,
                        cancellationToken);

                existingBook.SetCoverKey(
                    CoverKey.Create(generatedCoverKey));

                await _unitOfWork.SaveChangesAsync(
                    cancellationToken);
            }

            var alreadyAdded = await _userBooks.ExistsAsync(
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

        stream.Position = 0;

        var fileKey = await _fileStorage.UploadAsync(
            stream,
            request.File.FileName,
            request.File.ContentType,
            cancellationToken);

        var book = new Domain.Models.Book(
            BookTitle.Create(request.Title),
            request.Description,
            request.Author,
            FileKey.Create(fileKey),
            null,
            fileHash,
            request.File.Length,
            userId);

        CoverKey coverKey;

        if (request.Cover is not null &&
            request.Cover.Length > 0)
        {
            await using var coverStream =
                request.Cover.OpenReadStream();

            var uploadedCoverKey =
                await _fileStorage.UploadCoverAsync(
                    coverStream,
                    book.Id,
                    cancellationToken);

            coverKey = CoverKey.Create(
                uploadedCoverKey);
        }
        else
        {
            stream.Position = 0;

            await using var generatedCover =
                _coverGenerator.Generate(stream);

            var generatedCoverKey =
                await _fileStorage.UploadCoverAsync(
                    generatedCover,
                    book.Id,
                    cancellationToken);

            coverKey = CoverKey.Create(
                generatedCoverKey);
        }

        book.SetCoverKey(coverKey);

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

        return new UploadBookResult(
            book.Id,
            BookCreated: true,
            UserBookCreated: true);
    }
}