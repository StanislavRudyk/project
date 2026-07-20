using BookShare.Core.Extentions;

var builder = WebApplication.CreateBuilder(args);

builder
    .AddApplicationServices()
    .AddDependencyInjection()
    .AddDatabase();

var app = builder.Build();

app.UseApplicationMiddleware();
await app.RunApplicationAsync();