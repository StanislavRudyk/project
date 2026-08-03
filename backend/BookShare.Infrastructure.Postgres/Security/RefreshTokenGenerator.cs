using System.Globalization;
using System.Security.Cryptography;
using BookShare.Domain.Abstractions;

namespace BookShare.Infrastructure.Security;

public class RefreshTokenGenerator : IRefreshTokenGenerator
{

    public string Generate()
    {
        byte[] bytes = RandomNumberGenerator.GetBytes(32);
        return Convert.ToBase64String(bytes);
    }
}