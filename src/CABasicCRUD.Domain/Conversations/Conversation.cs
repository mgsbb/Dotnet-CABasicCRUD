using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Messages;
using CABasicCRUD.Domain.Users;

namespace CABasicCRUD.Domain.Conversations;

public sealed class Conversation : AggregateRoot<ConversationId>
{
    private readonly List<Message> _messages = new();
    private readonly List<ConversationParticipant> _participants = new();

    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();
    public IReadOnlyList<ConversationParticipant> Participants => _participants.AsReadOnly();
    public UserId CreatedById { get; private set; }
    public ConversationType ConversationType { get; private set; }

    private Conversation(ConversationId id, UserId createdById, ConversationType conversationType)
        : base(id)
    {
        CreatedById = createdById;
        ConversationType = conversationType;
    }

    public static Result<Conversation> Create(
        UserId createdById,
        IReadOnlyList<UserId> participantsUserIds
    )
    {
        Conversation conversation = new(ConversationId.New(), createdById, ConversationType.Group);

        foreach (UserId userId in participantsUserIds)
        {
            conversation._participants.Add(new ConversationParticipant(conversation.Id, userId));
        }

        return conversation;
    }

    public static Result<Conversation> CreatePrivate(UserId creatorId, UserId participantId)
    {
        if (creatorId == participantId)
        {
            return Result<Conversation>.Failure(ConversationErrors.CreatorSameAsParticipant);
        }

        Conversation conversation = new(ConversationId.New(), creatorId, ConversationType.Private);

        conversation._participants.Add(new ConversationParticipant(conversation.Id, creatorId));
        conversation._participants.Add(new ConversationParticipant(conversation.Id, participantId));

        return conversation;
    }

    public Result<Message> SendMessage(UserId senderUserId, string content)
    {
        if (
            !_participants.Any(conversationParticipant =>
                conversationParticipant.UserId == senderUserId
            )
        )
        {
            return Result<Message>.Failure(ConversationErrors.NotAParticipant);
        }

        Result<Message> result = Message.Create(senderUserId, content);

        if (result.IsFailure || result.Value is null)
        {
            return result;
        }

        _messages.Add(result.Value);

        return result;
    }
}
