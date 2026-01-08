using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts;

public sealed record PostResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
