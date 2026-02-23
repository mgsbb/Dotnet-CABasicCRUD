using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Common;

public interface IConversationReadService
{
    Task<IReadOnlyList<Conversation>> GetConversationsOfUser(UserId userId);

    Task<Conversation?> GetPrivateConversationAsync(
        UserId initiatorUserId,
        UserId participantUserId,
        CancellationToken cancellationToken
    );

    Task<Conversation?> GetByIdAsync(ConversationId conversationId);
}
