using System.Security.Claims;
using BookShare.Core.EndpointSettings;

namespace BookShare.Core.Endpoints.Book.GetBookFile;

public sealed class GetBookFileEndpoint : IEndpoint
{
    public void MapEndpoint(WebApplication app)
    {
        app.MapGet(
                "/api/books/{bookId:guid}/file",
                Handle)
            .WithTags("Books")
            .WithName("GetBookFile")
            .RequireAuthorization();
    }

    private static async Task<IResult> Handle(
        Guid bookId,
        HttpContext context,
        GetBookFileHandler handler,
        CancellationToken cancellationToken)
    {
        var userIdValue = context.User.FindFirstValue(
            ClaimTypes.NameIdentifier);

        if (!Guid.TryParse(userIdValue, out var userId))
            return Results.Unauthorized();

        var url = await handler.HandleAsync(
            userId,
            bookId,
            cancellationToken);

        if (url is null)
            return Results.NotFound();

        return Results.Redirect(url);
    }
}