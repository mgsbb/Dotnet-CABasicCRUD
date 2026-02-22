using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Common;

public sealed record PostWithAuthorResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
