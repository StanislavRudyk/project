using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Infrastructure.Postgres.Configuration;
using Microsoft.Extensions.Options;

namespace BookShare.Core.Endpoints.Auth.Refresh;

public sealed class RefreshHandler
{
    private readonly IUserRepository _users;
    private readonly IRefreshSessionRepository _refreshSessions;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;

    public RefreshHandler(
        IUserRepository users,
        IRefreshSessionRepository refreshSessions,
        IAccessTokenGenerator accessTokenGenerator,
        IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher,
        IUnitOfWork unitOfWork,
        IOptions<JwtOptions> jwtOptions)
    {
        _users = users;
        _refreshSessions = refreshSessions;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
        _unitOfWork = unitOfWork;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<RefreshResult> HandleAsync(string refreshToken)
    {
        var tokenHash = _tokenHasher.Hash(refreshToken);

        var session = await _refreshSessions.FindByTokenHashAsync(tokenHash);

        if (session is null)
            throw new UnauthorizedAccessException();

        if (session.IsRevoked)
        {
            throw new UnauthorizedAccessException();
        }

        if (session.IsExpired)
            throw new UnauthorizedAccessException();

        var user = await _users.FindByIdAsync(session.UserId);

        if (user is null)
            throw new UnauthorizedAccessException();

        var newAccessToken = _accessTokenGenerator.Generate(user);
        var newRefreshToken = _refreshTokenGenerator.Generate();

        var newSession = RefreshSession.Create(
            userId: user.Id,
            tokenHash: _tokenHasher.Hash(newRefreshToken),
            lifetime: TimeSpan.FromDays(_jwtOptions.RefreshTokenLifetimeDays),
            userAgent: session.UserAgent,
            ipAddress: session.IpAddress);

        session.Revoke(newSession.Id);

        await _refreshSessions.AddAsync(newSession);

        await _unitOfWork.SaveChangesAsync();
        
        return new RefreshResult(
            newAccessToken,
            newRefreshToken);
    }
}