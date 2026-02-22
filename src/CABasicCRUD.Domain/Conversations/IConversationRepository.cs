using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Conversations;

public interface IConversationRepository : IRepository<Conversation, ConversationId>
{
    Task<IReadOnlyList<Conversation>> GetConversationsOfUser(UserId userId);

    Task<Conversation?> GetPrivateConversationAsync(
        UserId initiatorUserId,
        UserId participantUserId,
        CancellationToken cancellationToken
    );
}
