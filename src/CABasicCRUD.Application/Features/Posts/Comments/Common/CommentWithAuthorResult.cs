using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Common;

public sealed record CommentWithAuthorResult(
    CommentId Id,
    string Body,
    PostId PostId,
    UserId UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
