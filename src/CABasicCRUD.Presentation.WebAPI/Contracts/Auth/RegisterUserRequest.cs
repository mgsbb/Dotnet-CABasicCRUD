namespace CABasicCRUD.Presentation.WebAPI.Contracts.Auth;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
