using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts;

public sealed record PostWithAuthorResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    string Username,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
