using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Auth.Login;

public sealed class RegistrationEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/auth/register", Handle)
            .WithTags("Authentication")
            .WithName("Register");
    }

    private static async Task<IResult> Handle(LoginRequest request)
    {
        return Results.Created($"/api/users/{id}", null);
    }
}