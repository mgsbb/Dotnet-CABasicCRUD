using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Users;

public sealed record UserResult(
    UserId Id,
    string Name,
    string Email,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
