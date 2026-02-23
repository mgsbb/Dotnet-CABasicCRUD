using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Queries.GetConversationsOfUser;

internal sealed class GetConversationsOfUserQueryHandler(
    ICurrentUser currentUser,
    IConversationReadService conversationReadService,
    IUserReadService userReadService
) : IQueryHander<GetConversationsOfUserQuery, IReadOnlyList<ConversationResultWithoutMessages>>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IConversationReadService _conversationReadService = conversationReadService;
    private readonly IUserReadService _userReadService = userReadService;

    public async Task<Result<IReadOnlyList<ConversationResultWithoutMessages>>> Handle(
        GetConversationsOfUserQuery request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<IReadOnlyList<ConversationResultWithoutMessages>>.Failure(
                AuthErrors.Unauthenticated
            );
        }

        UserId userId = (UserId)_currentUser.UserId;

        User? user = await _userReadService.GetByIdAsync(userId);

        if (user is null)
        {
            return Result<IReadOnlyList<ConversationResultWithoutMessages>>.Failure(
                Identity.Users.Common.UserErrors.NotFound
            );
        }

        IReadOnlyList<Conversation> conversations =
            await _conversationReadService.GetConversationsOfUser(userId);

        return Result<IReadOnlyList<ConversationResultWithoutMessages>>.Success(
            conversations.ToListConversationResultWithoutMessages()
        );
    }
}
