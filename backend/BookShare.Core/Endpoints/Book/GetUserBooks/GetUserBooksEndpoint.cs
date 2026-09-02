using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.GetUserBooks;

public sealed class GetUserBooksEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(
                "/api/users/{userId:guid}/books",
                Handle)
            .WithTags("Users")
            .WithName("GetUserBooks");
    }

    private static async Task<IResult> Handle(
        Guid userId,
        GetUserBooksHandler handler,
        CancellationToken cancellationToken)
    {
        var books = await handler.HandleAsync(
            userId,
            cancellationToken);

        return Results.Ok(books);
    }
}