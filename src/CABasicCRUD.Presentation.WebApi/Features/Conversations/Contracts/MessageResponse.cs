namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record MessageResponse(
    Guid Id,
    Guid SenderUserId,
    string Content,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
