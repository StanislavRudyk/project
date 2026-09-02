using BookShare.Core.EndpointSettings;
using Microsoft.AspNetCore.Mvc;

namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class UploadBookEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/books", Handle)
            .WithTags("Books")
            .WithName("UploadBook")
            .Accepts<UploadBookRequest>("multipart/form-data")
            .RequireAuthorization()
            .DisableAntiforgery();
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        [FromForm] UploadBookRequest request,
        UploadBookHandler handler,
        CancellationToken cancellationToken)
    {
        var result = await handler.HandleAsync(
            request,
            context,
            cancellationToken);

        return result.BookCreated
            ? Results.Created($"/api/books/{result.BookId}", result)
            : Results.Ok(result);
    }
}