using BookShare.Domain.Enums;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public static class BookFormatDetector
{
    public static BookFormat Detect(string fileName)
    {
        var extension = Path.GetExtension(fileName);

        return extension.ToLowerInvariant() switch
        {
            ".pdf" => BookFormat.Pdf,
            ".epub" => BookFormat.Epub,
            ".fb2" => BookFormat.Fb2,

            _ => throw new InvalidOperationException(
                "Неподдерживаемый формат книги.")
        };
    }
}