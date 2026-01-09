using CABasicCRUD.Domain.Messages;

namespace CABasicCRUD.Application.Features.Conversations;

internal static class MessageMappings
{
    internal static MessageResult ToMessageResult(this Message message)
    {
        return new MessageResult(
            message.Id,
            message.Content,
            message.SenderUserId,
            message.CreatedAt,
            message.UpdatedAt
        );
    }

    internal static IReadOnlyList<MessageResult> ToListMessageResult(
        this IReadOnlyList<Message> messages
    )
    {
        if (messages == null)
            return new List<MessageResult>();

        return messages.Select(message => message.ToMessageResult()).ToList();
    }
}
