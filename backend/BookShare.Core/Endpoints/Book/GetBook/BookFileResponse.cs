using BookShare.Domain.Enums;

namespace BookShare.Core.Endpoints.Book.GetBook;

public sealed record BookFileResponse(
    Guid Id,
    BookFormat Format,
    long FileSize);