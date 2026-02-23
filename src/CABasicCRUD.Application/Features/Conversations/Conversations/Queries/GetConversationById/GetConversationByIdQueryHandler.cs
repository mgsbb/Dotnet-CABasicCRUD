using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationById;

internal sealed class GetConversationByIdQueryHandler(
    IConversationReadService conversationReadService,
    ICurrentUser currentUser
) : IQueryHander<GetConversationByIdQuery, ConversationResult>
{
    private readonly IConversationReadService _conversationReadService = conversationReadService;
    private readonly ICurrentUser _currentUser = currentUser;

    public async Task<Result<ConversationResult>> Handle(
        GetConversationByIdQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<ConversationResult>.Failure(AuthErrors.Unauthenticated);
        }

        Conversation? conversation = await _conversationReadService.GetByIdAsync(request.Id);

        if (conversation is null)
        {
            return Result<ConversationResult>.Failure(Common.ConversationErrors.NotFound);
        }

        if (
            !conversation.Participants.Any(participant => participant.UserId == _currentUser.UserId)
        )
        {
            return Result<ConversationResult>.Failure(AuthErrors.Forbidden);
        }

        return Result<ConversationResult>.Success(conversation.ToConversationResult());
    }
}
