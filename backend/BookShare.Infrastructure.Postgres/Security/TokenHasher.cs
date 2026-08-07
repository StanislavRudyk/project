using System.Security.Cryptography;
using System.Text;
using BookShare.Domain.Abstractions;
using BookShare.Domain.ValueObject;

namespace BookShare.Infrastructure.Security;

public class TokenHasher : ITokenHasher
{
    public RefreshTokenHash Hash(string token)
    {
        var bytes = Encoding.UTF8.GetBytes(token);

        var hash = SHA256.HashData(bytes);

        return RefreshTokenHash.Create(
            Convert.ToHexString(hash));
    }
}