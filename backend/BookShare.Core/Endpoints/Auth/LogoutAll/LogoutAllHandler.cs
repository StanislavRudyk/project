using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Auth.LogoutAll;

public sealed class LogoutAllHandler
{
    private readonly IRefreshSessionRepository _refreshSessions;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutAllHandler(
        IRefreshSessionRepository refreshSessions,
        IUnitOfWork unitOfWork)
    {
        _refreshSessions = refreshSessions;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(Guid userId)
    {
        await _refreshSessions.RevokeAllAsync(userId);

        await _unitOfWork.SaveChangesAsync();
    }
}