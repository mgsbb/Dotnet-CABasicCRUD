using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Conversations.Conversations;

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
    public static readonly Error PrivateConversationInvalidParticipantCount = new(
        "Conversation.PrivateConversationInvalidParticipantCount",
        "A private conversation must have exactly 2 participants."
    );
    public static readonly Error CreatorMustBeParticipant = new(
        "Conversation.CreatorMustBeParticipant",
        "Creator of the private conversion must be a participant."
    );
}
