namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record CreateConversationRequest(
    IReadOnlyList<Guid> ParticipantUserIds,
    string? GroupTitle,
    string ConversationType
);
