using BookShare.Domain.Abstractions;
using BookShare.Domain.Enums;
using PDFtoImage;
using SkiaSharp;

namespace BookShare.Infrastructure.Postgres.Pdf;

public sealed class PdfBookCoverGenerator : IBookCoverGenerator
{
    public BookFormat Format => BookFormat.Pdf;

    public Task<Stream> GenerateAsync(
        Stream pdfStream,
        CancellationToken cancellationToken = default)
    {
        pdfStream.Position = 0;

        using var bitmap = Conversion.ToImage(
            pdfStream,
            page: 0);

        var output = new MemoryStream();

        bitmap.Encode(
            output,
            SKEncodedImageFormat.Jpeg,
            90);

        output.Position = 0;

        return Task.FromResult<Stream>(output);
    }
}