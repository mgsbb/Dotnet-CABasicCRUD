using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Comments;

public sealed record CommentId : EntityIdBase
{
    public CommentId(Guid guid)
        : base(guid) { }

    public static CommentId New() => new(Guid.NewGuid());

    public static explicit operator CommentId(Guid guid) => new(guid);
}
