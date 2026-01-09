namespace CABasicCRUD.Application.Features.Conversations;

public static class ConversationValidationErrorMessages
{
    public const string IdEmpty = "Conversation Id cannot be empty.";

    public const string MessageEmpty = "Message content cannot be empty.";

    public const string MessageTooLong = "Message cannot exceed 1000 characters.";
}
