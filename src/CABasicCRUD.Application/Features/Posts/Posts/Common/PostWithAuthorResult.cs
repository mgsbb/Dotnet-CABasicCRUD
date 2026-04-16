using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Posts.Common;

public sealed record PostWithAuthorResult(
    PostId Id,
    string Title,
    string Content,
    UserId UserId,
    string AuthorName,
    string? AuthorProfileImageUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<string> MediaUrls
);
