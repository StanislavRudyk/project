using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class User
{
    public Guid Id { get; private set; }
    public UserName UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public Email? Email { get; private set; }
    public DateTime CreatedAt { get; private set; }
    
    public UserModel(Guid id, UserName userName, string passwordHash, Email? email, DateTime createdAt)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        CreatedAt = createdAt;
    }
    
    public void ChangeEmail(Email newEmail)
    {
        Email = newEmail;
    }
}