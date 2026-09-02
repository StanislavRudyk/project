using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Domain.ValueObject;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.Repository;

public class BookRepository : IBookRepository
{
    private readonly DataContext _context;

    public BookRepository(DataContext context)
    {
        _context = context;
    }

    public async Task<Book?> FindByFileHashAsync(
        FileHash fileHash,
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .FirstOrDefaultAsync(
                book => book.FileHash == fileHash,
                cancellationToken);
    }

    public async Task<Book?> GetByIdAsync(
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        return await _context.Books
            .AsNoTracking()
            .FirstOrDefaultAsync(
                book => book.Id == bookId,
                cancellationToken);
    }

    public async Task AddAsync(
        Book book,
        CancellationToken cancellationToken = default)
    {
        await _context.Books.AddAsync(
            book,
            cancellationToken);
    }
}