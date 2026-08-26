using BookShare.Domain.Models;

namespace BookShare.Domain.Models;

public sealed class UserBook{
    public Guid UserId { get; private set; }
    public Guid BookId { get; private set; }
    public DateTimeOffset AddedAt { get; private set; }
    public bool IsFavorite { get; private set; }
    public User User { get; private set; } = null!;
    public Book Book { get; private set; } = null!;
    private UserBook()
    {
        
    }

    public UserBook(Guid userId, Guid bookId)
    {
        UserId = userId;
        BookId = bookId;
        AddedAt = DateTimeOffset.UtcNow;
    }
    
    public void AddToFavorites()
    {
        IsFavorite = true;
    }

    public void RemoveFromFavorites()
    {
        IsFavorite = false;
    }
} 
