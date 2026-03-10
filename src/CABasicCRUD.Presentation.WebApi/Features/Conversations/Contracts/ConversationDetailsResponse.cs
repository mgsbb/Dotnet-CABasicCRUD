using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record ConversationDetailsResponse(
    Guid Id,
    ConversationType ConversationType,
    IReadOnlyList<ConversationParticipantDetailResponse> Participants,
    IReadOnlyList<MessageDetailResponse> Messages,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);
