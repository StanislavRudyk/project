using BookShare.Domain.ValueObject;

namespace BookShare.Core.Endpoints.Auth.Login;

public sealed record LoginRequest(
    string UserName,
    string Password);