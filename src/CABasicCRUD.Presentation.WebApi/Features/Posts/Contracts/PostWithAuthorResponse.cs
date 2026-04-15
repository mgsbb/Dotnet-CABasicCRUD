namespace CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

public record PostWithAuthorResponse(
    Guid Id,
    string Title,
    string Content,
    Guid UserId,
    string AuthorName,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<string> MediaUrls
);
