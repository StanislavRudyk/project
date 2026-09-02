namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed class UploadBookRequest
{
    public IFormFile File { get; init; } = null!;

    public IFormFile? Cover { get; init; }

    public string Title { get; init; } = null!;

    public string? Description { get; init; }

    public string? Author { get; init; }
}