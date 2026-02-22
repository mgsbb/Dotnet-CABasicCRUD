using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationById;

public sealed record GetConversationByIdQuery(ConversationId Id) : IQuery<ConversationResult>;
