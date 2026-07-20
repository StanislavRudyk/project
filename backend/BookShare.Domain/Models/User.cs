using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class User
{
    public Guid Id { get; private set; }
    public UserName UserName { get; private set; }
    public PasswordHash PasswordHash { get; private set; }
    public Email? Email { get; private set; }
    public DateTimeOffset CreatedAt { get; private set; }

    private User()
    {
    }

    private User(
        Guid id,
        UserName userName,
        PasswordHash passwordHash,
        Email? email,
        DateTimeOffset createdAt)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        CreatedAt = createdAt;
    }

    public static User Create(
        UserName userName,
        PasswordHash passwordHash,
        Email? email)
    {
        return new User(
            Guid.NewGuid(),
            userName,
            passwordHash,
            email,
            DateTimeOffset.UtcNow);
    }

    public void ChangeEmail(Email? email)
    {
        Email = email;
    }

    public void ChangePassword(PasswordHash passwordHash)
    {
        PasswordHash = passwordHash;
    }
}