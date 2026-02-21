using CABasicCRUD.Domain.Comments;
using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Comments;

public sealed record CommentWithAuthorResult(
    CommentId Id,
    string Body,
    PostId PostId,
    UserId UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
