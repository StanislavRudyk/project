using BCrypt.Net;
using BookShare.Domain.Abstractions;
using BookShare.Domain.ValueObject;

namespace BookShare.Infrastructure.Security;

public sealed class PasswordHasher : IPasswordHasher
{
    public PasswordHash GenerateHash(string password)
    {
        var hash = BCrypt.Net.BCrypt.HashPassword(password);

        return PasswordHash.Create(hash);
    }

    public bool VerifyHash(
        PasswordHash hash,
        string password)
    {
        return BCrypt.Net.BCrypt.Verify(password, hash.Value);
    }
}