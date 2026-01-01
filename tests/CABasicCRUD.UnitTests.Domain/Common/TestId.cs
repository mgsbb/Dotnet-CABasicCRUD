using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.UnitTests.Domain.Common;

public sealed record TestId : EntityIdBase
{
    private TestId(Guid guid)
        : base(guid) { }

    public static TestId New() => new(Guid.NewGuid());

    public static explicit operator TestId(Guid guid) => new(guid);
}
