using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Auth.Common;

public sealed record AuthResult(
    UserId Id,
    string Name,
    string Email,
    string Token,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
