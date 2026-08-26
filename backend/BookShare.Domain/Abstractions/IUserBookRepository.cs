using BookShare.Domain.Models;

namespace BookShare.Domain.Abstractions;

public interface IUserBookRepository
{
    Task<bool> ExistsAsync(
        Guid userId,
        Guid bookId,
        CancellationToken cancellationToken = default);

    Task AddAsync(
        UserBook userBook,
        CancellationToken cancellationToken = default);
}