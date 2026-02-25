using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace CABasicCRUD.Infrastructure.Chats;

public sealed class ChatHub(ILogger<ChatHub> logger) : Hub
{
    public async Task JoinConversation(string conversationId)
    {
        logger.LogInformation("Joined conversation: {conversationId}", conversationId);

        await Groups.AddToGroupAsync(Context.ConnectionId, conversationId);
    }

    public async Task LeaveConversation(string conversationId)
    {
        logger.LogInformation("Left conversation: {conversationId}", conversationId);

        await Groups.RemoveFromGroupAsync(Context.ConnectionId, conversationId);
    }
}
