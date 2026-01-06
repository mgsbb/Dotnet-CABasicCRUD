namespace CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

public record PostResponse(
    Guid Id,
    string Title,
    string Content,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
