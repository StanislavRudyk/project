namespace BookShare.Core.Endpoints.Book.UploadBook;

public sealed record UploadBookResult(
    Guid BookId,
    bool BookCreated,
    bool UserBookCreated
);