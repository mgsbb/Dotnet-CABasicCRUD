namespace CABasicCRUD.Presentation.WebAPI.Auth.Contracts;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
