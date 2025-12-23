namespace CABasicCRUD.Application.Auth.DTOs;

public sealed record AuthResult(Guid Id, string Name, string Email, string Token);
