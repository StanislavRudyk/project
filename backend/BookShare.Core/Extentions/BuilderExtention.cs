using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using BookShare.Core.Endpoints.Auth.Login;
using BookShare.Core.Endpoints.Auth.Logout;
using BookShare.Core.Endpoints.Auth.LogoutAll;
using BookShare.Core.Endpoints.Auth.Refresh;
using BookShare.Core.Endpoints.Auth.Registration;
using BookShare.Core.Endpoints.Book.AddBook;
using BookShare.Core.Endpoints.Book.DeleteBook;
using BookShare.Core.Endpoints.Book.GetBook;
using BookShare.Core.Endpoints.Book.GetBookCover;
using BookShare.Core.Endpoints.Book.GetBookFile;
using BookShare.Core.Endpoints.Book.GetBooks;
using BookShare.Core.Endpoints.Book.UploadBook;
using BookShare.Core.Endpoints.Book.GetUserBooks;
using BookShare.Core.EndpointSettings;
using BookShare.Core.Settings;
using BookShare.Domain.Abstractions;
using BookShare.Infrastructure.Postgres.Configuration;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using BookShare.Infrastructure.Postgres.Pdf;
using BookShare.Infrastructure.Postgres.Repositories;
using BookShare.Infrastructure.Postgres.Repository;
using BookShare.Infrastructure.Security;
using BookShare.Infrastructure.Storage;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Minio;

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

        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
        });

        builder.Services.AddResponseCompression(options =>
        {
            options.EnableForHttps = true;
        });

        builder.Services.AddOpenApi(options =>
        {
            options.AddDocumentTransformer((document, context, cancellationToken) =>
            {
                document.Info.Title = "BookShare API";
                document.Info.Version = "v1";
                document.Info.Description = "REST API для BookShare";

                return Task.CompletedTask;
            });
        });
        
        builder.Services.AddAntiforgery();
        
        builder.Services.ConfigureHttpJsonOptions(options =>
        {
            options.SerializerOptions.Converters.Add(
                new JsonStringEnumConverter());
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
    
    public static WebApplicationBuilder AddDependencyInjection(this WebApplicationBuilder builder)
    {
            // Handlers
            builder.Services.AddScoped<RegistrationHandler>();
            builder.Services.AddScoped<LoginHandler>();
            builder.Services.AddScoped<RefreshHandler>();
            builder.Services.AddScoped<LogoutHandler>();
            builder.Services.AddScoped<LogoutAllHandler>();
            builder.Services.AddScoped<UploadBookHandler>();
            builder.Services.AddScoped<GetBooksHandler>();
            builder.Services.AddScoped<GetBookHandler>();
            builder.Services.AddScoped<AddBookHandler>();
            builder.Services.AddScoped<DeleteBookHandler>();
            builder.Services.AddScoped<GetUserBooksHandler>();
            builder.Services.AddScoped<GetBookFileHandler>();
            builder.Services.AddScoped<GetBookCoverHandler>();
            builder.Services.AddScoped<BookCoverGenerator>();
            builder.Services.AddScoped<BookUploadPreparationService>();

            // Repositories
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IRefreshSessionRepository, RefreshSessionRepository>();
            builder.Services.AddScoped<IBookRepository, BookRepository>();
            builder.Services.AddScoped<IUserBookRepository, UserBookRepository>();

            // Security
            builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();
            builder.Services.AddScoped<IAccessTokenGenerator, AccessTokenGenerator>();
            builder.Services.AddScoped<IRefreshTokenGenerator, RefreshTokenGenerator>();
            builder.Services.AddScoped<ITokenHasher, TokenHasher>();

            // Unit of Work
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();

            builder.Services.AddScoped<DbContext, DataContext>();
            builder.Services.AddScoped<IBookCoverGenerator, PdfBookCoverGenerator>();

            builder.Services.Configure<JwtOptions>(
                builder.Configuration.GetSection(JwtOptions.SectionName));

            // Forwarded Headers
            builder.Services.Configure<ForwardedHeadersOptions>(options =>
            {
                options.ForwardedHeaders =
                    ForwardedHeaders.XForwardedFor |
                    ForwardedHeaders.XForwardedProto;

                options.KnownNetworks.Clear();
                options.KnownProxies.Clear();
            });
            
            builder.Services.Configure<MinioOptions>(
                builder.Configuration.GetSection(
                    MinioOptions.SectionName));

            builder.Services.AddSingleton<IMinioClient>(sp =>
            {
                var options = sp
                    .GetRequiredService<IOptions<MinioOptions>>()
                    .Value;

                return new MinioClient()
                    .WithEndpoint(options.Endpoint)
                    .WithCredentials(
                        options.AccessKey,
                        options.SecretKey)
                    .WithSSL(options.UseSsl)
                    .Build();
            });

            builder.Services.AddScoped<
                IBookFileStorage,
                MinioBookFileStorage>();
            
            return builder;
    }
    
    
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
    
    public static WebApplicationBuilder AddAuthenticationServices(
        this WebApplicationBuilder builder)
    {
        var jwt = builder.Configuration
                      .GetSection(JwtOptions.SectionName)
                      .Get<JwtOptions>()
                  ?? throw new InvalidOperationException("JwtOptions not configured.");

        builder.Services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,

                    ValidIssuer = jwt.Issuer,
                    ValidAudience = jwt.Audience,

                    IssuerSigningKey = new SymmetricSecurityKey(
                        Encoding.UTF8.GetBytes(jwt.SecretKey))
                };

                options.Events = new JwtBearerEvents
                {
                    OnMessageReceived = context =>
                    {
                        context.Token = context.Request.Cookies["access_token"];
                        return Task.CompletedTask;
                    }
                };
            });

        builder.Services.AddAuthorization();

        return builder;
    }

    public static WebApplicationBuilder AddHealthChecks(
    this WebApplicationBuilder builder)
    {
        builder.Services.AddHealthChecks();

        return builder;
    }
}