using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Messages.Common;
using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.Messages.Commands.SendMessage;

public sealed record SendMessageCommand(ConversationId ConversationId, string Content)
    : ICommand<MessageResult>;
