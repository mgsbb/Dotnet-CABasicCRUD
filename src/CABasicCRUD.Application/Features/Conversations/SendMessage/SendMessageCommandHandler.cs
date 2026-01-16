using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Auth;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;
using CABasicCRUD.Domain.Messages;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Application.Features.Conversations.SendMessage;

internal sealed class SendMessageCommandHandler(
    IConversationRepository conversationRepository,
    ICurrentUser currentUser,
    IUnitOfWork unitOfWork
) : ICommandHandler<SendMessageCommand, MessageResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly ICurrentUser _currentUser = currentUser;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

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
            return Result<MessageResult>.Failure(ConversationErrors.NotFound);
        }

        Result<Message> result = conversation.SendMessage(
            (UserId)_currentUser.UserId,
            request.Content
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<MessageResult>.Failure(result.Error);
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<MessageResult>.Success(result.Value.ToMessageResult());
    }
}
