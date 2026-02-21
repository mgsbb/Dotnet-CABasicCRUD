using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Posts;

public sealed record PostWithAuthorResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
