namespace CABasicCRUD.Presentation.WebApi.Features.Posts.Contracts;

public record PostWithAuthorResponse(
    Guid Id,
    string Title,
    string Content,
    Guid UserId,
    string AuthorName,
    string? AuthorProfileImageUrl,
    DateTime CreatedAt,
    DateTime? UpdatedAt,
    IReadOnlyList<string> MediaUrls
);
