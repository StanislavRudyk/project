namespace BookShare.Core.Endpoints.Auth.Registration;

public record RegistrationRequest(
    string Username,
    string Password,
    string? Email);