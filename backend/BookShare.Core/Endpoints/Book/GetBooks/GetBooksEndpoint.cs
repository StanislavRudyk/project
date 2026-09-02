using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.GetBooks;

public sealed class GetBooksEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/books", Handle)
            .WithTags("Books")
            .WithName("GetBooks")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        HttpContext context,
        GetBooksHandler handler,
        CancellationToken cancellationToken)
    {
        var books = await handler.HandleAsync(
            context,
            cancellationToken);

        return Results.Ok(books);
    }
}