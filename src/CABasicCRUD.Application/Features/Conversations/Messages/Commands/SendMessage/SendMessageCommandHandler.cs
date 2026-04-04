using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Conversations.Messages.Common;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Conversations;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Application.Features.Conversations.Messages.Commands.SendMessage;

internal sealed class SendMessageCommandHandler(
    IConversationRepository conversationRepository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork,
    IChatNotificationService chatNotificationService,
    IUserReadService userReadService
) : ICommandHandler<SendMessageCommand, MessageResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;
    private readonly IChatNotificationService _chatNotificationService = chatNotificationService;
    private readonly IUserReadService _userReadService = userReadService;

    public async Task<Result<MessageResult>> Handle(
        SendMessageCommand request,
        CancellationToken cancellationToken
    )
    {
        if (!_currentUser.IsAuthenticated)
        {
            return Result<MessageResult>.Failure(AuthErrors.Unauthenticated);
        }

        Conversation? conversation = await _conversationRepository.GetByIdAsync(
            request.ConversationId
        );

        if (conversation is null)
        {
            return Result<MessageResult>.Failure(Conversations.Common.ConversationErrors.NotFound);
        }

        Result<Message> result = conversation.SendMessage(
            (UserId)_currentUser.UserId,
            request.Content
        );

        if (result.IsFailure)
        {
            return Result<MessageResult>.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        User? user = await _userReadService.GetByIdAsync(result.Value.SenderUserId);

        await _chatNotificationService.NotifyNewMessage(
            conversation.Id,
            result.Value.Id,
            result.Value.Content,
            result.Value.SenderUserId,
            user!.Username,
            user!.UserProfile.FullName,
            result.Value.CreatedAt,
            cancellationToken
        );

        return Result<MessageResult>.Success(result.Value.ToMessageResult());
    }
}
