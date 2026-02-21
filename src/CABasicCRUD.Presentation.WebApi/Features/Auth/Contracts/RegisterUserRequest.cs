namespace CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;

public sealed record RegisterUserRequest(
    string Name,
    string Username,
    string Email,
    string Password
);
