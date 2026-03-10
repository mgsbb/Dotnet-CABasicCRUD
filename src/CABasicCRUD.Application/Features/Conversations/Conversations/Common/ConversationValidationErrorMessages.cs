namespace CABasicCRUD.Application.Features.Conversations.Conversations.Common;

public static class ConversationValidationErrorMessages
{
    public const string IdEmpty = "Conversation Id cannot be empty.";

    public const string MessageEmpty = "Message content cannot be empty.";

    public const string MessageTooLong = "Message cannot exceed 1000 characters.";

    public const string ConversationTypeEmpty = "Conversation type cannot be empty.";

    public const string ConversationTypeInvalid = "Conversation must be 'Private' or 'Group'";
}
