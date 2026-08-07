using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using BookShare.Infrastructure.Postgres.Configuration;
using Microsoft.Extensions.Options;

namespace BookShare.Core.Endpoints.Auth.Login;

public sealed class LoginHandler
{
    private readonly IUserRepository _users;
    private readonly IPasswordHasher _passwordHasher;
    private readonly IAccessTokenGenerator _accessTokenGenerator;
    private readonly IRefreshTokenGenerator _refreshTokenGenerator;
    private readonly ITokenHasher _tokenHasher;
    private readonly IRefreshSessionRepository _refreshSessions;
    private readonly IUnitOfWork _unitOfWork;
    private readonly JwtOptions _jwtOptions;

    public LoginHandler(IUserRepository users, IPasswordHasher passwordHasher,
        IAccessTokenGenerator accessTokenGenerator, IRefreshTokenGenerator refreshTokenGenerator,
        ITokenHasher tokenHasher, IRefreshSessionRepository refreshSessions,
        IUnitOfWork unitOfWork, IOptions<JwtOptions> jwtOptions)
    {
        _users = users;
        _passwordHasher = passwordHasher;
        _accessTokenGenerator = accessTokenGenerator;
        _refreshTokenGenerator = refreshTokenGenerator;
        _tokenHasher = tokenHasher;
        _refreshSessions = refreshSessions;
        _unitOfWork = unitOfWork;
        _jwtOptions = jwtOptions.Value;
    }

    public async Task<LoginResult> HandleAsync(LoginRequest request, HttpContext context)
    {
        // TODO: Доработать поиск одновременно по почте и по логину
        UserName userName = UserName.Create(request.UserName);
        User? user = await _users.FindByUserNameAsync(userName);

        if (user is null)
            throw new UnauthorizedAccessException();
        
        if (!_passwordHasher.VerifyHash(user.PasswordHash, request.Password))
            throw new UnauthorizedAccessException();
            
        var accessToken = _accessTokenGenerator.Generate(user);
        var refreshToken = _refreshTokenGenerator.Generate();
        var refreshTokenHash = _tokenHasher.Hash(refreshToken);
        
        string? userAgent = context.Request.Headers.UserAgent.ToString();
        if (string.IsNullOrWhiteSpace(userAgent))
            userAgent = null;
        string? ipAddress = context.Connection.RemoteIpAddress?.ToString();
        
        var session = RefreshSession.Create(
            userId: user.Id,
            tokenHash: refreshTokenHash,
            lifetime: TimeSpan.FromDays(_jwtOptions.RefreshTokenLifetimeDays),
            userAgent: userAgent,
            ipAddress: ipAddress);
        

        
        await _refreshSessions.AddAsync(session);
        await _unitOfWork.SaveChangesAsync();
        
        return new LoginResult(
            accessToken,
            refreshToken);
    }
}