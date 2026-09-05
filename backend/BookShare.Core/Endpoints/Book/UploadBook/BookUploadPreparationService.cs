using System.Security.Cryptography;
using BookShare.Domain.Enums;
using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class BookUploadPreparationService
{
    private readonly BookCoverGenerator _coverGenerator;

    public BookUploadPreparationService(
        BookCoverGenerator coverGenerator)
    {
        _coverGenerator = coverGenerator;
    }

    public async Task<BookUploadPreparationResult> PrepareAsync(
        UploadBookRequest request,
        CancellationToken cancellationToken = default)
    {
        // Проверка файла
        if (request.File is null || request.File.Length == 0)
        {
            throw new InvalidOperationException(
                "File is empty.");
        }

        var format = BookFormatDetector.Detect(
            request.File.FileName);

        var hasUploadedCover =
            request.Cover is not null &&
            request.Cover.Length > 0;

        if (!hasUploadedCover &&
            format != BookFormat.Pdf)
        {
            throw new InvalidOperationException(
                $"Для формата {format} необходимо загрузить обложку.");
        }

        await using var bookStream =
            request.File.OpenReadStream();

        var hashBytes = await SHA256.HashDataAsync(
            bookStream,
            cancellationToken);

        var fileHash = FileHash.Create(
            Convert.ToHexString(hashBytes));

        Stream coverStream;

        if (hasUploadedCover)
        {
            coverStream = new MemoryStream();

            await using var uploadedCover =
                request.Cover!.OpenReadStream();

            await uploadedCover.CopyToAsync(
                coverStream,
                cancellationToken);

            coverStream.Position = 0;
        }
        else
        {
            bookStream.Position = 0;

            coverStream =
                await _coverGenerator.GenerateAsync(
                    format,
                    bookStream,
                    cancellationToken);

            coverStream.Position = 0;
        }

        return new BookUploadPreparationResult(
            format,
            fileHash,
            coverStream);
    }
}
