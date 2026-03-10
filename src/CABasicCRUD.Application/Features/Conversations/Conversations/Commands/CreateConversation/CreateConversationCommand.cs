using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;

public sealed record CreateConversationCommand(
    UserId CreatorUserId,
    IReadOnlyList<UserId> ParticipantIds,
    ConversationType ConversationType,
    string? GroupTitle
) : ICommand<ConversationResult>;
