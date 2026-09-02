using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.GetBook;

public sealed class GetBookEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/books/{bookId:guid}", Handle)
            .WithTags("Books")
            .WithName("GetBook")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid bookId,
        GetBookHandler handler,
        CancellationToken cancellationToken)
    {
        var book = await handler.HandleAsync(
            bookId,
            cancellationToken);

        return book is null
            ? Results.NotFound()
            : Results.Ok(book);
    }
}