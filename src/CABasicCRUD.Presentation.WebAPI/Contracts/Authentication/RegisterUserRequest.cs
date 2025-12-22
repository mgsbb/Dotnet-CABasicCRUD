namespace CABasicCRUD.Presentation.WebAPI.Contracts.Authentication;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
