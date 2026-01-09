namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record ConversationResponse(
    Guid Id,
    IReadOnlyList<Guid> ParticipantsId,
    IReadOnlyList<MessageResponse> Messages,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
