namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record ConversationResponseWithoutMessages(
    Guid Id,
    IReadOnlyList<Guid> ParticipantsId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
