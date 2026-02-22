using CABasicCRUD.Application.Features.Conversations.Messages.Common;
using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Common;

public sealed record ConversationResult(
    ConversationId Id,
    IReadOnlyList<Guid> ParticipantsId,
    IReadOnlyList<MessageResult> Messages,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
