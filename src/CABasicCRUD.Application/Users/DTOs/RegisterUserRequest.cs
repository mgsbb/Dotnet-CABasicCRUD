namespace CABasicCRUD.Application.Users.DTOs;

public sealed record RegisterUserRequest(string Name, string Email, string Password);
