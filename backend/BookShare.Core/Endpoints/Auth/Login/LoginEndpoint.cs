using BookShare.Core.EndpointSettings;

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
        LoginRequest request,
        LoginHandler handler)
    {
        var response = await handler.HandleAsync(request);

        return Results.Ok(response);
    }
}