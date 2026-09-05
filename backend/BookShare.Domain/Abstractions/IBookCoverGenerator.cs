using BookShare.Domain.Enums;

namespace BookShare.Domain.Abstractions;

public interface IBookCoverGenerator
{
    BookFormat Format { get; }

    Task<Stream> GenerateAsync(
        Stream bookStream,
        CancellationToken cancellationToken = default);
}