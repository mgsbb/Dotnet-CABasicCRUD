using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations.StartPrivateConversation;

public sealed record StartPrivateConversationCommand(
    UserId InitiatorUserId,
    UserId ParticipantUserId
) : ICommand<ConversationResult>;
