using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Identity.Users.Common;

public sealed record UserResult(
    UserId Id,
    string Name,
    string Email,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
