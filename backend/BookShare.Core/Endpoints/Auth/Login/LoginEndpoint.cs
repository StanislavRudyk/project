using BookShare.Core.EndpointSettings;
using BookShare.Infrastructure.Postgres.Configuration;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Options;

namespace BookShare.Core.Endpoints.Auth.Login;

public sealed class LoginEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/login", Handle)
            .WithTags("Authentication")
            .WithName("Login");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        LoginRequest request,
        LoginHandler handler,
        IOptions<JwtOptions> options)
    {
        var jwtOptions = options.Value;
        var result = await handler.HandleAsync(request, context);

        context.Response.Cookies.Append(
            "access_token",
            result.AccessToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Lax,
                Expires = DateTimeOffset.UtcNow.AddMinutes(jwtOptions.AccessTokenLifetimeMinutes)
            });

        context.Response.Cookies.Append(
            "refresh_token",
            result.RefreshToken,
            new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddDays(jwtOptions.RefreshTokenLifetimeDays)
            });

        return Results.Ok();
    }
}