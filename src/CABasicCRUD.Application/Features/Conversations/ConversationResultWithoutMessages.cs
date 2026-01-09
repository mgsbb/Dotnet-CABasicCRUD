using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations;

public sealed record ConversationResultWithoutMessages(
    ConversationId Id,
    IReadOnlyList<Guid> ParticipantsId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
