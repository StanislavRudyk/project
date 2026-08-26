namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed record UploadBookRequest(
    IFormFile File,
    string Title,
    string? Description,
    string? Author
);