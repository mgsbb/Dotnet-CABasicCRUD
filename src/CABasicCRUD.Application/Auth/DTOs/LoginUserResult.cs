namespace CABasicCRUD.Application.Auth.DTOs;

public sealed record LoginUserResult(Guid Id, string Name, string Email, string Token);
