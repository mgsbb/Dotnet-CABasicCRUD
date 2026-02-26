using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Queries;

public sealed record GetConversationByIdWithDetailsQuery(ConversationId ConversationId)
    : IQuery<ConversationDetailsResult>;

public sealed record ConversationDetailsResult(
    ConversationId Id,
    ConversationType ConversationType,
    IReadOnlyList<ConversationParticipantDetail> Participants,
    IReadOnlyList<MessageDetail> Messages,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record MessageDetail(
    MessageId Id,
    string Content,
    UserId SenderUserId,
    string SenderUsername,
    string SenderFullName,
    DateTime CreatedAt,
    DateTime? UpdatedAt
);

public sealed record ConversationParticipantDetail(
    UserId ParticipantUserId,
    string ParticipantUsername,
    string ParticipantFullName
);

internal sealed class GetConversationByIdWithDetailsQueryHandler(
    IConversationReadService conversationReadService,
    ICurrentUser currentUser
) : IQueryHander<GetConversationByIdWithDetailsQuery, ConversationDetailsResult>
{
    public async Task<Result<ConversationDetailsResult>> Handle(
        GetConversationByIdWithDetailsQuery request,
        CancellationToken cancellationToken
    )
    {
        ConversationDetailsResult? conversation =
            await conversationReadService.GetConversationByIdWithDetails(request.ConversationId);

        if (conversation is null)
        {
            return Result<ConversationDetailsResult>.Failure(Common.ConversationErrors.NotFound);
        }

        if (
            !conversation.Participants.Any(participant =>
                participant.ParticipantUserId == currentUser.UserId
            )
        )
        {
            return Result<ConversationDetailsResult>.Failure(
                Domain.Conversations.Conversations.ConversationErrors.NotAParticipant
            );
        }

        return conversation;
    }
}
