using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Conversations;

public static class ConversationErrors
{
    public static readonly Error ConversationWithSelf = new(
        "Conversation.WithSelf",
        "Cannot create a conversation with self."
    );

    public static readonly Error NotFound = new(
        "Conversation.NotFound",
        "Conversation with the given Id not found."
    );
}
