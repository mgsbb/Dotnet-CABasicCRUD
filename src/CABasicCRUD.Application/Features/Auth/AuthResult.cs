namespace CABasicCRUD.Application.Features.Auth;

public sealed record AuthResult(Guid Id, string Name, string Email, string Token);
