using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Weather;

public class WeatherForecast : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/me", (HttpContext context) =>
            {
                var id = context.User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier);

                return Results.Ok(id?.Value);
            })
            .RequireAuthorization();
    }
}