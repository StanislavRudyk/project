namespace BookShare.Infrastructure.Postgres.Configuration;

public sealed class JwtOptions
{
    public const string SectionName = "Jwt";

    public string Issuer { get; init; } = string.Empty;

    public string Audience { get; init; } = string.Empty;

    public string SecretKey { get; init; } = string.Empty;

    public int AccessTokenLifetimeMinutes { get; init; }

    public int RefreshTokenLifetimeDays { get; init; }
}