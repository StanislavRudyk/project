using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Health;

public sealed class HealthEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/health", () =>
        {
            return Results.Ok(new
            {
                status = "healthy"
            });
        });
    }
}