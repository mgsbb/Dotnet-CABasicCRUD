namespace CABasicCRUD.Application.Users.DTOs;

public sealed record LoginUserResult(Guid Id, string Name, string Email, string Token);
