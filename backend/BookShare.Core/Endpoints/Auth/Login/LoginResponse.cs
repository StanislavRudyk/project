namespace BookShare.Core.Endpoints.Auth.Login;

public sealed record LoginResponse(    
    string AccessToken,
    string RefreshToken);