using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.AddBook;

public sealed class AddBookEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapPost("/api/books/me/{bookId:guid}", Handle)
            .WithTags("Books")
            .WithName("AddBook")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid bookId,
        HttpContext context,
        AddBookHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(
            bookId,
            context,
            cancellationToken);

        return Results.NoContent();
    }
}