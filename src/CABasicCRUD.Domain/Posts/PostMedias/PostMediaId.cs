using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.PostMedias;

public sealed record PostMediaId : EntityIdBase
{
    public PostMediaId(Guid id)
        : base(id) { }

    public static PostMediaId New() => new(Guid.NewGuid());

    public static explicit operator PostMediaId(Guid id) => new(id);
}
