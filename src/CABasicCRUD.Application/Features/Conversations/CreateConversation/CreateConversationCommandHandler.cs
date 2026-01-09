using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations.CreateConversation;

internal sealed class CreateConversationCommandHandler(
    IConversationRepository conversationRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IUserRepository userRepository
) : ICommandHandler<CreateConversationCommand, ConversationResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserRepository _userRepository = userRepository;

    public async Task<Result<ConversationResult>> Handle(
        CreateConversationCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<ConversationResult>.Failure(AuthErrors.Unauthenticated);
        }

        if (_currentUser.UserId == request.UserId)
        {
            return Result<ConversationResult>.Failure(ConversationErrors.ConversationWithSelf);
        }

        User? user = await _userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<ConversationResult>.Failure(Users.UserErrors.NotFound);
        }

        Result<Conversation> result = Conversation.Create(
            (UserId)_currentUser.UserId,
            [(UserId)_currentUser.UserId, request.UserId]
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<ConversationResult>.Failure(result.Error);
        }

        await _conversationRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ConversationResult>.Success(result.Value.ToConversationResult());
    }
}
