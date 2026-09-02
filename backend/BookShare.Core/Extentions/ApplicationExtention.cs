using Scalar.AspNetCore;

namespace BookShare.Core.Extentions;

public static class ApplicationExtention
{
    public static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.MapOpenApi();

            app.MapScalarApiReference(options =>
            {
                options.Title = "BookShare API";
            });

            app.MapGet("/", () => Results.Redirect("/scalar"));
        }

        app.UseHttpsRedirection();

        app.UseForwardedHeaders();
        app.UseCors("AllowAll");
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseAntiforgery();

        app.MapEndpoints();
        app.MapControllers();

        return app;
    }

    public static async Task RunApplicationAsync(this WebApplication app)
    {
        await app.RunAsync();
    }
}