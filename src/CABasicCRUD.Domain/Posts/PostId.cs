using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts;

public record PostId : EntityIdBase
{
    private PostId(Guid guid)
        : base(guid) { }

    public static PostId New() => new(Guid.NewGuid());

    public static explicit operator PostId(Guid guid) => new(guid);
}
