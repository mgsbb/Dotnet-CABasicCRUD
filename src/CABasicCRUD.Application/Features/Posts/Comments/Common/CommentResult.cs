using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.Comments;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Common;

public sealed record CommentResult(
    CommentId Id,
    string Body,
    PostId PostId,
    UserId UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
