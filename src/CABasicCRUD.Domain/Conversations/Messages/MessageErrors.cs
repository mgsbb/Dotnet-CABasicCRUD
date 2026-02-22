using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Conversations.Messages;

public static class MessageErrors
{
    public static readonly Error ContentEmpty = new(
        "Message.ContentEmpty",
        "Message content cannot be empty."
    );
}
