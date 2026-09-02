using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.DeleteBook;

public sealed class DeleteBookEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapDelete("/api/books/{bookId:guid}", Handle)
            .WithTags("Books")
            .WithName("DeleteBook")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid bookId,
        HttpContext context,
        DeleteBookHandler handler,
        CancellationToken cancellationToken)
    {
        await handler.HandleAsync(
            bookId,
            context,
            cancellationToken);

        return Results.NoContent();
    }
}