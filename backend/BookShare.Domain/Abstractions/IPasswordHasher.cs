using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Abstractions;

public interface IPasswordHasher
{
    PasswordHash GenerateHash(string password);
    bool VerifyHash(PasswordHash hash, string password);
}