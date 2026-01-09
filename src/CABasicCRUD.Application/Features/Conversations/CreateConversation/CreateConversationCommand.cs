using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations.CreateConversation;

public sealed record CreateConversationCommand(UserId UserId) : ICommand<ConversationResult>;
