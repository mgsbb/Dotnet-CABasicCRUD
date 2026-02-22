using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;

namespace CABasicCRUD.Domain.Conversations.Messages;

public sealed class Message : EntityBase<MessageId>
{
    public UserId SenderUserId { get; }
    public string Content { get; }

    private Message(MessageId id, UserId senderUserId, string content)
        : base(id)
    {
        SenderUserId = senderUserId;
        Content = content;
    }

    public static Result<Message> Create(UserId senderUserId, string content)
    {
        if (string.IsNullOrWhiteSpace(content))
        {
            return Result<Message>.Failure(MessageErrors.ContentEmpty);
        }

        Message message = new(MessageId.New(), senderUserId, content);
        return message;
    }
}
