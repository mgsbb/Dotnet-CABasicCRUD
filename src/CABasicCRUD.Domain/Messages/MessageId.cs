using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Messages;

public sealed record MessageId : EntityIdBase
{
    private MessageId(Guid value)
        : base(value) { }

    public static MessageId New() => new(Guid.NewGuid());

    public static explicit operator MessageId(Guid value) => new(value);
}
