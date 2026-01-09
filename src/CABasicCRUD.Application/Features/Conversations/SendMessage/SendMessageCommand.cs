using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.SendMessage;

public sealed record SendMessageCommand(ConversationId ConversationId, string Content)
    : ICommand<MessageResult>;
