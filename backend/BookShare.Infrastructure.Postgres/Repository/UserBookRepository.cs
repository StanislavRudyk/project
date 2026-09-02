using BookShare.Domain.Abstractions;
using BookShare.Domain.Models;
using BookShare.Infrastructure.Postgres.DatabaseSettings;
using Microsoft.EntityFrameworkCore;

namespace BookShare.Infrastructure.Postgres.Repositories;

public sealed class UserBookRepository : IUserBookRepository
{
    private readonly DataContext _context;

    public UserBookRepository(DataContext context)
    {
        _context = context;
    }

    public Task<bool> ExistsAsync(
        Guid userId,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        return _context.UserBooks.AnyAsync(
            userBook =>
                userBook.UserId == userId &&
                userBook.BookId == bookId,
            cancellationToken);
    }

    public async Task AddAsync(
        UserBook userBook,
        CancellationToken cancellationToken = default)
    {
        await _context.UserBooks.AddAsync(
            userBook,
            cancellationToken);
    }
    
    public async Task<IReadOnlyList<Book>> GetBooksByUserIdAsync(
        Guid userId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserBooks
            .Where(ub => ub.UserId == userId)
            .Select(ub => ub.Book)
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }
    
    public async Task<Book?> GetBookByUserIdAsync(
        Guid userId,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        return await _context.UserBooks
            .Where(ub =>
                ub.UserId == userId &&
                ub.BookId == bookId)
            .Select(ub => ub.Book)
            .AsNoTracking()
            .FirstOrDefaultAsync(cancellationToken);
    }
    
    public async Task DeleteAsync(
        Guid userId,
        Guid bookId,
        CancellationToken cancellationToken = default)
    {
        var userBook = await _context.UserBooks
            .FirstOrDefaultAsync(
                ub => ub.UserId == userId &&
                      ub.BookId == bookId,
                cancellationToken);

        if (userBook is not null)
        {
            _context.UserBooks.Remove(userBook);
        }
    }
}