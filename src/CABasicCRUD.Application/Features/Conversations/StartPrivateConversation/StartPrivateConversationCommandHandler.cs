using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations;

namespace CABasicCRUD.Application.Features.Conversations.StartPrivateConversation;

internal sealed class StartPrivateConversationCommandHandler(
    IConversationRepository conversationRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<StartPrivateConversationCommand, ConversationResult>
{
    private readonly IConversationRepository _conversationRepository = conversationRepository;
    private readonly IUnitOfWork _unitOfWork = unitOfWork;

    public async Task<Result<ConversationResult>> Handle(
        StartPrivateConversationCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.InitiatorUserId == request.ParticipantUserId)
        {
            return Result<ConversationResult>.Failure(ConversationErrors.ConversationWithSelf);
        }

        Conversation? exisitingConversation =
            await _conversationRepository.GetPrivateConversationAsync(
                request.InitiatorUserId,
                request.ParticipantUserId,
                cancellationToken
            );

        if (exisitingConversation is not null)
        {
            return exisitingConversation.ToConversationResult();
        }

        Result<Conversation> result = Conversation.CreatePrivate(
            request.InitiatorUserId,
            request.ParticipantUserId
        );

        if (result.IsFailure || result.Value is null)
        {
            return Result<ConversationResult>.Failure(result.Error);
        }

        await _conversationRepository.AddAsync(result.Value);

        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return result.Value.ToConversationResult();
    }
}
