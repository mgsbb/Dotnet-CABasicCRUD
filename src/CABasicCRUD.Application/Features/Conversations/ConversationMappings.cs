using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations;

internal static class ConversationMappings
{
    internal static ConversationResult ToConversationResult(this Conversation conversation)
    {
        var v = conversation.Messages;
        return new ConversationResult(
            conversation.Id,
            conversation.Participants.Select(cp => cp.UserId.Value).ToList(),
            conversation.Messages.ToListMessageResult(),
            conversation.CreatedAt,
            conversation.UpdatedAt
        );
    }

    internal static IReadOnlyList<ConversationResult> ToListConversationResult(
        this IReadOnlyList<Conversation> conversations
    )
    {
        if (conversations == null)
            return new List<ConversationResult>();

        return conversations.Select(conversation => conversation.ToConversationResult()).ToList();
    }

    internal static ConversationResultWithoutMessages ToConversationResultWithoutMessages(
        this Conversation conversation
    )
    {
        var v = conversation.Messages;
        return new ConversationResultWithoutMessages(
            conversation.Id,
            conversation.Participants.Select(cp => cp.UserId.Value).ToList(),
            conversation.CreatedAt,
            conversation.UpdatedAt
        );
    }

    internal static IReadOnlyList<ConversationResultWithoutMessages> ToListConversationResultWithoutMessages(
        this IReadOnlyList<Conversation> conversations
    )
    {
        if (conversations == null)
            return new List<ConversationResultWithoutMessages>();

        return conversations
            .Select(conversation => conversation.ToConversationResultWithoutMessages())
            .ToList();
    }
}
