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
    IUserReadService userReadService,
    IConversationReadService conversationReadService
) : ICommandHandler<CreateConversationCommand, ConversationResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUserReadService _userReadService = userReadService;
    private readonly IConversationReadService _conversationReadService = conversationReadService;

    public async Task<Result<ConversationResult>> Handle(
        CreateConversationCommand request,
        CancellationToken cancellationToken
    )
    {
        if (_currentUser.UserId != request.CreatorUserId)
        {
            return Result<ConversationResult>.Failure(AuthErrors.Forbidden);
        }

        if (request.ConversationType == ConversationType.Private)
        {
            if (request.ParticipantIds.Count != 2)
            {
                return Result<ConversationResult>.Failure(
                    ConversationErrors.InvalidParticipantCount
                );
            }

            Conversation? exisitingConversation =
                await _conversationReadService.GetPrivateConversationAsync(
                    request.ParticipantIds[0],
                    request.ParticipantIds[1],
                    cancellationToken
                );

            if (exisitingConversation is not null)
            {
                return exisitingConversation.ToConversationResult();
            }
        }

        Result<Conversation> result = Conversation.Create(
            (UserId)_currentUser.UserId,
            request.ParticipantIds,
            request.ConversationType,
            request.GroupTitle
        );

        if (result.IsFailure)
        {
            return Result<ConversationResult>.Failure(result.Error);
        }

        await _conversationRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<ConversationResult>.Success(result.Value.ToConversationResult());
    }
}
