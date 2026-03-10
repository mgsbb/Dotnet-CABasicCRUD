using System.Text.RegularExpressions;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Conversations.Messages;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Domain.Conversations.Conversations;

public sealed class Conversation : AggregateRoot<ConversationId>
{
    private readonly List<Message> _messages = new();
    private readonly List<ConversationParticipant> _participants = new();

    public IReadOnlyList<Message> Messages => _messages.AsReadOnly();
    public IReadOnlyList<ConversationParticipant> Participants => _participants.AsReadOnly();
    public UserId CreatedById { get; private set; }
    public ConversationType ConversationType { get; private set; }

    public string? GroupTitle { get; private set; }

    private Conversation(
        ConversationId id,
        UserId createdById,
        ConversationType conversationType,
        string? groupTitle
    )
        : base(id)
    {
        CreatedById = createdById;
        ConversationType = conversationType;
        GroupTitle = groupTitle;
    }

    public static Result<Conversation> Create(
        UserId createdById,
        IReadOnlyList<UserId> participantsUserIds,
        ConversationType conversationType,
        string? groupTitle
    )
    {
        if (!participantsUserIds.Contains(createdById))
        {
            return Result<Conversation>.Failure(ConversationErrors.CreatorMustBeParticipant);
        }

        // already being checked in the handler
        if (conversationType == ConversationType.Private && participantsUserIds.Count != 2)
        {
            return Result<Conversation>.Failure(
                ConversationErrors.PrivateConversationInvalidParticipantCount
            );
        }

        Conversation conversation;

        if (conversationType == ConversationType.Private)
        {
            conversation = new(ConversationId.New(), createdById, conversationType, null);
        }
        else
        {
            conversation = new(ConversationId.New(), createdById, conversationType, groupTitle);
        }

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

        Conversation conversation = new(
            ConversationId.New(),
            creatorId,
            ConversationType.Private,
            null
        );

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

        if (result.IsFailure)
        {
            return result;
        }

        _messages.Add(result.Value);

        return result;
    }
}
