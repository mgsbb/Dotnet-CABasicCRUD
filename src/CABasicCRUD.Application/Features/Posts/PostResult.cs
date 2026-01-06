namespace CABasicCRUD.Application.Features.Posts;

public sealed record PostResult(
    Guid Id,
    string Title,
    string Content,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
