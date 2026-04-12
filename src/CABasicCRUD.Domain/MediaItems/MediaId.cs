using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.MediaItems;

public sealed record MediaId : EntityIdBase
{
    private MediaId(Guid id)
        : base(id) { }

    public static MediaId New() => new(Guid.NewGuid());

    public static explicit operator MediaId(Guid id) => new(id);
}
