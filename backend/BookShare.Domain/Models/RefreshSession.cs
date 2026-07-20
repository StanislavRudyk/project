using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class RefreshSession
{
    public Guid Id { get; private set; }

    public Guid UserId { get; private set; }

    public RefreshTokenHash TokenHash { get; private set; }

    public DateTime CreatedAt { get; private set; }

    public DateTime ExpiresAt { get; private set; }

    public DateTime? RevokedAt { get; private set; }

    public Guid? ReplacedBySessionId { get; private set; }

    public string? UserAgent { get; private set; }

    public string? IpAddress { get; private set; }

    public bool IsExpired => ExpiresAt <= DateTime.UtcNow;

    public bool IsRevoked => RevokedAt is not null;

    public bool IsActive => !IsExpired && !IsRevoked;

    private RefreshSession()
    {
    }

    private RefreshSession(
        Guid id,
        Guid userId,
        RefreshTokenHash tokenHash,
        DateTime createdAt,
        DateTime expiresAt,
        string? userAgent,
        string? ipAddress)
    {
        Id = id;
        UserId = userId;
        TokenHash = tokenHash;
        CreatedAt = createdAt;
        ExpiresAt = expiresAt;
        UserAgent = userAgent;
        IpAddress = ipAddress;
    }

    public static RefreshSession Create(
        Guid userId,
        RefreshTokenHash tokenHash,
        TimeSpan lifetime,
        string? userAgent,
        string? ipAddress)
    {
        var now = DateTime.UtcNow;

        return new RefreshSession(
            Guid.NewGuid(),
            userId,
            tokenHash,
            now,
            now.Add(lifetime),
            userAgent,
            ipAddress);
    }

    public void Revoke(Guid? replacedBySessionId = null)
    {
        if (IsRevoked)
            throw new InvalidOperationException("Session has already been revoked.");

        RevokedAt = DateTime.UtcNow;
        ReplacedBySessionId = replacedBySessionId;
    }
}