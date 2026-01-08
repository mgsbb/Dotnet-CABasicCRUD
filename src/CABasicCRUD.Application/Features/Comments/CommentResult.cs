using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments;

public sealed record CommentResult(
    CommentId Id,
    string Body,
    PostId PostId,
    UserId UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
