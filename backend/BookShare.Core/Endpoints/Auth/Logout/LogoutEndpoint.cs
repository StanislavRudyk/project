using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Auth.Logout;

public sealed class LogoutEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/logout", Handle)
            .WithTags("Authentication")
            .WithName("Logout");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        LogoutHandler handler)
    {
        context.Request.Cookies.TryGetValue(
            "refresh_token",
            out var refreshToken);

        if (!string.IsNullOrEmpty(refreshToken))
        {
            await handler.HandleAsync(refreshToken);
        }

        context.Response.Cookies.Delete("access_token");

        context.Response.Cookies.Delete("refresh_token");

        return Results.NoContent();
    }
}