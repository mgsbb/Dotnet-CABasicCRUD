using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Conversations.Common;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Identity.Users;
using ConversationErrors = CABasicCRUD.Application.Features.Conversations.Conversations.Common.ConversationErrors;

namespace CABasicCRUD.Application.Features.Conversations.Conversations.Commands.CreateConversation;

internal sealed class CreateConversationCommandHandler(
    IConversationRepository conversationRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser,
    IUserReadService userReadService
) : ICommandHandler<CreateConversationCommand, ConversationResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserReadService _userReadService = userReadService;

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

        User? user = await _userReadService.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<ConversationResult>.Failure(ConversationErrors.NotFound);
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
