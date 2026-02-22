using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Conversations;

public static class ConversationErrors
{
    public static readonly Error NotAParticipant = new(
        "Conversation.NotAParticipant",
        "User with the given Id is not a participant of the conversation."
    );
    public static readonly Error CreatorSameAsParticipant = new(
        "Conversation.CreatorSameAsParticipant",
        "Creator user id is the same as the participant user id."
    );
}
