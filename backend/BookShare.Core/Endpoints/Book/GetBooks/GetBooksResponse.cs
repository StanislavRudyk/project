namespace BookShare.Core.Endpoints.Book.GetBooks;

public sealed record GetBooksResponse(
    Guid Id,
    string Title,
    string? Description,
    string? Author,
    string? CoverKey,
    long FileSize,
    DateTimeOffset CreatedAt
);