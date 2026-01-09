using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Conversations;

public sealed record ConversationId : EntityIdBase
{
    private ConversationId(Guid value)
        : base(value) { }

    public static ConversationId New() => new(Guid.NewGuid());

    public static explicit operator ConversationId(Guid value) => new(value);
}
