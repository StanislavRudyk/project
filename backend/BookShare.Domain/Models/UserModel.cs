using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Models;

public sealed class UserModel
{
    public Guid Id { get; private set; }
    public UserName UserName { get; private set; }
    public string PasswordHash { get; private set; }
    public Email Email { get; private set; }
    public DateTime CreateAt { get; private set; }
    
    public UserModel(Guid id, UserName userName, string passwordHash, Email email, DateTime createAt)
    {
        Id = id;
        UserName = userName;
        PasswordHash = passwordHash;
        Email = email;
        CreateAt = createAt;
    }
    
    public void ChangeEmail(Email newEmail)
    {
        Email = newEmail;
    }
}