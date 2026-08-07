namespace BookShare.Core.Endpoints.Auth.Login;

public sealed record LoginResult(
    string AccessToken,
    string RefreshToken);