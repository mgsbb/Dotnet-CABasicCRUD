namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record MessageDetailResponse(
    Guid Id,
    string Content,
    Guid SenderUserId,
    string SenderUsername,
    string SenderFullName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
