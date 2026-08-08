using BookShare.Core.EndpointSettings;
using System.Security.Claims;

namespace BookShare.Core.Endpoints.Auth.LogoutAll;

public sealed class LogoutAllEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/logout-all", Handle)
            .RequireAuthorization()
            .WithTags("Authentication")
            .WithName("LogoutAll");
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        LogoutAllHandler handler)
    {
        var userIdClaim = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdClaim, out var userId))
            return Results.Unauthorized();

        await handler.HandleAsync(userId);

        context.Response.Cookies.Delete("access_token");
        context.Response.Cookies.Delete("refresh_token");

        return Results.NoContent();
    }
}