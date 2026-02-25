using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;
using Microsoft.AspNetCore.SignalR;

namespace CABasicCRUD.Infrastructure.Chats;

public sealed class ChatNotificationService(IHubContext<ChatHub> hubContext)
    : IChatNotificationService
{
    private readonly IHubContext<ChatHub> _hubContext = hubContext;

    public Task NotifyNewMessage(
        ConversationId conversationId,
        MessageId messageId,
        UserId senderId,
        string senderUsername,
        string senderFullName,
        string content,
        DateTime sentAt,
        CancellationToken cancellationToken
    )
    {
        return _hubContext
            .Clients.Group(conversationId.Value.ToString())
            .SendAsync(
                "MessageReceived",
                new
                {
                    MessageId = messageId.Value,
                    ConversationId = conversationId.Value,
                    SenderId = senderId.Value,
                    SenderUsername = senderUsername,
                    SenderFullName = senderFullName,
                    Content = content,
                    SentAt = sentAt,
                },
                cancellationToken
            );
    }
}
