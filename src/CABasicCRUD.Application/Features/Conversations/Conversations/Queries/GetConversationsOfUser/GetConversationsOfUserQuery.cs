using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationsOfUser;

public sealed record GetConversationsOfUserQuery()
    : IQuery<IReadOnlyList<ConversationResultWithoutMessages>>;
