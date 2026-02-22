using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Common;

public sealed record ConversationResultWithoutMessages(
    ConversationId Id,
    IReadOnlyList<Guid> ParticipantsId,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
