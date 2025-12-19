using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Users;

public sealed record UserId : EntityIdBase
{
    private UserId(Guid guid)
        : base(guid) { }

    public static UserId New() => new(Guid.NewGuid());

    public static explicit operator UserId(Guid guid) => new(guid);
}
