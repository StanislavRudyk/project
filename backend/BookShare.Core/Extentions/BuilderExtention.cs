using System.Text.Json;
using BookShare.Core.EndpointSettings;
using BookShare.Core.Settings;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace BookShare.Core.Extentions;

public static class BuilderExtention
{
    public static WebApplicationBuilder AddApplicationServices(this WebApplicationBuilder builder)
    {
        builder.Services.AddControllers()    
        .AddJsonOptions(options =>
        {
            options.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "BookShare API",
                Version = "v1",
                Description = "API",
                License = new OpenApiLicense
                {
                    Name = "MIT"
                }
            });
            
            
            // Когда будет JWT
            // options.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            // {
            //     Type = SecuritySchemeType.ApiKey,
            //     Name = "Cookie",
            //     In = ParameterLocation.Cookie,
            //     Description = "JWT Authorization header"
            // });
        });

        return builder;
    }
    
    public static WebApplicationBuilder AddDatabase(this WebApplicationBuilder builder)
    {
        var databaseSettings = builder.Configuration
            .GetSection(DatabaseOptions.SectionName)
            .Get<DatabaseOptions>();

        if (string.IsNullOrEmpty(databaseSettings?.ConnectionString))
        {
            throw new InvalidOperationException($"Configuration section '{DatabaseOptions.SectionName}' is missing or incomplete.");
        }

        builder.Services.AddDbContext<DataContext>(options =>
            options.UseNpgsql(databaseSettings.ConnectionString));

        return builder;
    }
    
    // public static WebApplicationBuilder AddDependencyInjection(this WebApplicationBuilder builder)
    // {
    //     return builder;
    // }
    
    
    // Политики CORS
    public static WebApplicationBuilder AddCorsPolicy(this WebApplicationBuilder builder, string policyName = "AllowAll")
    {
        builder.Services.AddCors(options =>
        {
            options.AddPolicy(policyName, policy =>
            {
                policy.WithOrigins("http://localhost:5500", 
                 "http://localhost:5173",
                 "http://localhost:5174", 
                 "http://localhost:5074",
                 "http://127.0.0.1:5500")
                      .AllowAnyMethod()
                      .AllowAnyHeader()
                      .AllowCredentials();
            });
        });

        return builder;
    }

    // Мапинг эндпоинтов
    public static IApplicationBuilder MapEndpoints(this WebApplication app)
    {
        var endpointTypes = typeof(Program).Assembly.GetTypes()
            .Where(t => typeof(IEndpoint).IsAssignableFrom(t) && !t.IsInterface && !t.IsAbstract);

        foreach (var endpointType in endpointTypes)
        {
            var endpoint = Activator.CreateInstance(endpointType) as IEndpoint;
            endpoint?.MapEndpoint(app);
        }

        return app;
    }
}