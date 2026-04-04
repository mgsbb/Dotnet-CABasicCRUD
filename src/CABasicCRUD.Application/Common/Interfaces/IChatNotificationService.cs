using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IChatNotificationService
{
    Task NotifyNewMessage(
        ConversationId conversationId,
        MessageId messageId,
        string content,
        UserId senderUserId,
        string senderUsername,
        string senderFullName,
        DateTime sentAt,
        CancellationToken cancellationToken
    );
}
