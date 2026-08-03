using BookShare.Domain.ValueObject;

namespace BookShare.Domain.Abstractions;

public interface ITokenHasher
{
    RefreshTokenHash Hash(string token);
}