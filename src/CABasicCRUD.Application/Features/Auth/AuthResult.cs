using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Auth;

public sealed record AuthResult(
    UserId Id,
    string Name,
    string Email,
    string Token,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
