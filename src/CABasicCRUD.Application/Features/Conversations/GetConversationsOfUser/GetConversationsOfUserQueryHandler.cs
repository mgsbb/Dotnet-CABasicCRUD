using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations.GetConversationsOfUser;

internal sealed class GetConversationsOfUserQueryHandler(
    ICurrentUser currentUser,
    IConversationRepository conversationRepository,
    IUserRepository userRepository
) : IQueryHander<GetConversationsOfUserQuery, IReadOnlyList<ConversationResultWithoutMessages>>
{
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IConversationRepository _conversationsRepository = conversationRepository;
    private readonly IUserRepository _userRepository = userRepository;

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

        User? user = await _userRepository.GetByIdAsync(userId);

        if (user is null)
        {
            return Result<IReadOnlyList<ConversationResultWithoutMessages>>.Failure(
                Users.UserErrors.NotFound
            );
        }

        IReadOnlyList<Conversation> conversations =
            await _conversationsRepository.GetConversationsOfUser(userId);

        return Result<IReadOnlyList<ConversationResultWithoutMessages>>.Success(
            conversations.ToListConversationResultWithoutMessages()
        );
    }
}
