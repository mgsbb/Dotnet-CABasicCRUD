namespace CABasicCRUD.Presentation.WebApi.Features.Auth.Contracts;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
