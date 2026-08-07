using BookShare.Core.EndpointSettings;
using BookShare.Infrastructure.Postgres.Configuration;
using Microsoft.Extensions.Options;

namespace BookShare.Core.Endpoints.Auth.Refresh;

public sealed class RefreshEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/refresh", Handle)
            .WithTags("Authentication")
            .WithName("Refresh");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        RefreshHandler handler,
        IOptions<JwtOptions> options)
    {
        if (!context.Request.Cookies.TryGetValue(
                "refresh_token",
                out var refreshToken))
        {
            return Results.Unauthorized();
        }

        var result = await handler.HandleAsync(refreshToken);

        var jwt = options.Value;

        context.Response.Cookies.Append(
            "access_token",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow
                    .AddMinutes(jwt.AccessTokenLifetimeMinutes)
            });

        context.Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow
                    .AddDays(jwt.RefreshTokenLifetimeDays)
            });

        return Results.NoContent();
    }
}