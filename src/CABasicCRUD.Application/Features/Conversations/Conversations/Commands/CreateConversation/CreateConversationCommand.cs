using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;

public sealed record CreateConversationCommand(UserId UserId) : ICommand<ConversationResult>;
