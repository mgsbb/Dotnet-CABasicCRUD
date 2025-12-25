namespace CABasicCRUD.Presentation.WebAPI.Features.Auth.Contracts;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
