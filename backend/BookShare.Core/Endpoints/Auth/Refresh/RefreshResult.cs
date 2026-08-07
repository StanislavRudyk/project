namespace BookShare.Core.Endpoints.Auth.Refresh;

public sealed record RefreshResult(
    string AccessToken,
    string RefreshToken);