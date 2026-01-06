namespace CABasicCRUD.Presentation.WebApi.Features.Comments.Contracts;

public record CommentResponse(
    Guid Id,
    string Body,
    Guid PostId,
    Guid UserId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
