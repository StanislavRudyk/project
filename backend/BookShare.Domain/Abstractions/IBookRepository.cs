using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Abstractions;

public interface IBookRepository
{
    Task<Book?> FindByFileHashAsync(
        FileHash fileHash,
        CancellationToken cancellationToken);

    Task<Book?> GetByIdAsync(
        Guid bookId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default);
}