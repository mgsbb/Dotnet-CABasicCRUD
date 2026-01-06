using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments;

public sealed record CommentResult(
    Guid Id,
    string Body,
    PostId PostId,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
