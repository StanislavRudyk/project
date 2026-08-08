using BookShare.Domain.Abstractions;

namespace BookShare.Core.Endpoints.Auth.Logout;

public sealed class LogoutHandler
{
    private readonly IRefreshSessionRepository _refreshSessions;
    private readonly ITokenHasher _tokenHasher;
    private readonly IUnitOfWork _unitOfWork;

    public LogoutHandler(
        IRefreshSessionRepository refreshSessions,
        ITokenHasher tokenHasher,
        IUnitOfWork unitOfWork)
    {
        _refreshSessions = refreshSessions;
        _tokenHasher = tokenHasher;
        _unitOfWork = unitOfWork;
    }

    public async Task HandleAsync(string refreshToken)
    {
        var tokenHash = _tokenHasher.Hash(refreshToken);

        var session = await _refreshSessions.FindByTokenHashAsync(tokenHash);

        if (session is null)
            return;

        if (!session.IsRevoked)
        {
            session.Revoke();

            await _unitOfWork.SaveChangesAsync();
        }
    }
}