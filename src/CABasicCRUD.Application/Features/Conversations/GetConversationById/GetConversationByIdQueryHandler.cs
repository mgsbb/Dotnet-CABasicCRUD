using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.GetConversationById;

internal sealed class GetConversationByIdQueryHandler(
    IConversationRepository conversationRepository,
    ICurrentUser currentUser
) : IQueryHander<GetConversationByIdQuery, ConversationResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
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

        Conversation? conversation = await _conversationRepository.GetByIdAsync(request.Id);

        if (conversation is null)
        {
            return Result<ConversationResult>.Failure(ConversationErrors.NotFound);
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
