using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Common;

public sealed record PostResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<string> MediaUrls
);
