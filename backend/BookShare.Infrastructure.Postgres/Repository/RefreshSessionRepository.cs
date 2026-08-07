using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.Repository;

public sealed class RefreshSessionRepository : IRefreshSessionRepository
{
    private readonly DataContext _context;

    public RefreshSessionRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<RefreshSession?> FindByTokenHashAsync(
        RefreshTokenHash hash)
    {
        return await _context.RefreshSessions
            .FirstOrDefaultAsync(x => x.TokenHash == hash);
    }

    public async Task<IReadOnlyList<RefreshSession>> GetActiveByUserIdAsync(
        Guid userId)
    {
        return await _context.RefreshSessions
            .Where(x =>
                x.UserId == userId &&
                x.RevokedAt == null &&
                x.ExpiresAt > DateTime.UtcNow)
            .ToListAsync();
    }

    public async Task AddAsync(RefreshSession session)
    {
        await _context.RefreshSessions.AddAsync(session);
    }

    public Task UpdateAsync(RefreshSession session)
    {
        _context.RefreshSessions.Update(session);
        return Task.CompletedTask;
    }

    public async Task RevokeAllAsync(Guid userId)
    {
        var sessions = await _context.RefreshSessions
            .Where(x => x.UserId == userId && x.RevokedAt == null)
            .ToListAsync();

        foreach (var session in sessions)
        {
            session.Revoke();
        }
    }
}