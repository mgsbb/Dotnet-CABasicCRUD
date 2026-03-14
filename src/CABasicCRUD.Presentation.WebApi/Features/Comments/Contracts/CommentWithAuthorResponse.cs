namespace CABasicCRUD.Presentation.WebApi.Features.Comments.Contracts;

public record CommentWithAuthorResponse(
    Guid Id,
    string Body,
    Guid PostId,
    Guid UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
