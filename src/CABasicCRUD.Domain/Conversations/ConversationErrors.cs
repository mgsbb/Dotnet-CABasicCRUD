using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Conversations;

public static class ConversationErrors
{
    public static readonly Error NotAParticipant = new(
        "Conversation.NotAParticipant",
        "User with the given Id is not a participant of the conversation."
    );
}
