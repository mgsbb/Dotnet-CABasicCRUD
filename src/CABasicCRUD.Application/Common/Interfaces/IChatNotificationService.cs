using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IChatNotificationService
{
    Task NotifyNewMessage(
        ConversationId conversationId,
        MessageId messageId,
        UserId senderId,
        string senderUsername,
        string senderFullName,
        string content,
        DateTime sentAt,
        CancellationToken cancellationToken
    );
}
