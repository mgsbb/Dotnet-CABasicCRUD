namespace CABasicCRUD.Presentation.WebApi.Features.Conversations.Contracts;

public sealed record ConversationParticipantDetailResponse(
    Guid ParticipantUserId,
    string ParticipantUsername,
    string ParticipantFullName
);
