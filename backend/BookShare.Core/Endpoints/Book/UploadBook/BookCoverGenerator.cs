using BookShare.Domain.Abstractions;
using BookShare.Domain.Enums;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class BookCoverGenerator
{
    private readonly IEnumerable<IBookCoverGenerator> _generators;

    public BookCoverGenerator(
        IEnumerable<IBookCoverGenerator> generators)
    {
        _generators = generators;
    }

    public async Task<Stream> GenerateAsync(
        BookFormat format,
        Stream bookStream,
        CancellationToken cancellationToken = default)
    {
        var generator = _generators
            .FirstOrDefault(x => x.Format == format);

        if (generator is null)
        {
            throw new InvalidOperationException(
                $"Генератор обложки для формата {format} не найден.");
        }

        return await generator.GenerateAsync(
            bookStream,
            cancellationToken);
    }
}