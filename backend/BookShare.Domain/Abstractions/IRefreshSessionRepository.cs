using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Abstractions;

public interface IRefreshSessionRepository
{
    Task<RefreshSession?> FindByTokenHashAsync(
        RefreshTokenHash hash);

    Task<IReadOnlyList<RefreshSession>> FindByUserIdAsync(
        Guid userId);

    Task AddAsync(RefreshSession session);

    Task UpdateAsync(RefreshSession session);

    Task RevokeAllAsync(Guid userId);
}