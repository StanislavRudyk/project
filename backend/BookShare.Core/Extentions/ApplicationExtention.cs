namespace BookShare.Core.Extentions;

public static class ApplicationExtention
{
    public static WebApplication UseApplicationMiddleware(this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "TODO Sharp API v1");
                options.RoutePrefix = string.Empty;
            });
        }

        app.UseHttpsRedirection();
        app.UseAuthentication();
        app.UseAuthorization();
        app.MapEndpoints();
        app.MapControllers();

        return app;
    }

    public static async Task RunApplicationAsync(this WebApplication app)
    {
        await app.RunAsync();
    }
}