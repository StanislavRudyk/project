using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Weather;

public class WeatherForecast : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("weather/weatherforecast", () =>
            {
                var summaries = new[]
                {
                    "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
                };
                return Results.Json(summaries);
            })
            .WithTags("Weather");
    }
}