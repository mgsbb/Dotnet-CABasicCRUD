using CABasicCRUD.Application.Common.Interfaces.Messaging;

namespace CABasicCRUD.Application.Features.Conversations.GetConversationsOfUser;

public sealed record GetConversationsOfUserQuery()
    : IQuery<IReadOnlyList<ConversationResultWithoutMessages>>;
