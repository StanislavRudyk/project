namespace BookShare.Domain.Abstractions;

public interface IRefreshTokenGenerator
{
    string Generate();
}