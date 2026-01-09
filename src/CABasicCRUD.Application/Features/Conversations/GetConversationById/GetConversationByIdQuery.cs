using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.GetConversationById;

public sealed record GetConversationByIdQuery(ConversationId Id) : IQuery<ConversationResult>;
