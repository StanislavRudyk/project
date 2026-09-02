using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Book.GetBook;

public sealed record GetBookResponse(
    Guid Id,
    string Title,
    string? Description,
    string? Author,
    CoverKey? CoverKey,
    IReadOnlyList<BookFileResponse> Files,
    DateTimeOffset CreatedAt);