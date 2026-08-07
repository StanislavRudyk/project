using BookShare.Domain.Models;

namespace BookShare.Domain.Abstractions;

public interface IAccessTokenGenerator
{
    string Generate(User user);
}