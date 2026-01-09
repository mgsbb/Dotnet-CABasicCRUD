using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations;

public sealed record ConversationResult(
    ConversationId Id,
    IReadOnlyList<Guid> ParticipantsId,
    IReadOnlyList<MessageResult> Messages,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
