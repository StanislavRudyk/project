using PDFtoImage;
using SkiaSharp;

namespace BookShare.Infrastructure.Postgres.Pdf;

public sealed class PdfCoverGenerator
{
    public Stream Generate(Stream pdfStream)
    {
        pdfStream.Position = 0;

        var bitmap = Conversion.ToImage(
            pdfStream,
            page: 0);

        var output = new MemoryStream();

        bitmap.Encode(
            output,
            SKEncodedImageFormat.Jpeg,
            90);

        output.Position = 0;

        bitmap.Dispose();

        return output;
    }
}