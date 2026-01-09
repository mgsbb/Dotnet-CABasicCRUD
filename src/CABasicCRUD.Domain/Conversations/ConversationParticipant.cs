using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Conversations;

public sealed class ConversationParticipant
{
    public ConversationId ConversationId { get; private set; }
    public UserId UserId { get; private set; }

    internal ConversationParticipant(ConversationId conversationId, UserId userId)
    {
        ConversationId = conversationId;
        UserId = userId;
    }
}
