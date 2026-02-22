using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.StartPrivateConversation;

public sealed record StartPrivateConversationCommand(
    UserId InitiatorUserId,
    UserId ParticipantUserId
) : ICommand<ConversationResult>;
