using BookShare.Core.Extentions;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApplicationServices()
    .AddDatabase()
    .AddDependencyInjection()
    .AddAuthenticationServices()
    .AddCorsPolicy();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<DataContext>();

    await db.Database.MigrateAsync();
}

app.UseApplicationMiddleware();
await app.RunApplicationAsync();
