using BookShare.Domain.Models;

namespace BookShare.Domain.Abstractions;

public interface IBookRepository
{
    Task<Book?> FindByFileHashAsync(
        string fileHash,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default);
}