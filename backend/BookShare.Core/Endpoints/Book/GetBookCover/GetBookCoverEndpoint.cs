using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.GetBookCover;

public sealed class GetBookCoverEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet("/api/books/{bookId:guid}/cover", Handle)
            .WithTags("Books")
            .WithName("GetBookCover")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid bookId,
        GetBookCoverHandler handler,
        CancellationToken cancellationToken)
    {
        var cover = await handler.HandleAsync(
            bookId,
            cancellationToken);

        return cover is null
            ? Results.NotFound()
            : Results.File(
                cover,
                "image/jpeg");
    }
}