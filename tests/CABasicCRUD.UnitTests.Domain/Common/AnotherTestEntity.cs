using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.UnitTests.Domain.Common;

public class AnotherTestEntity : EntityBase<TestId>
{
    private AnotherTestEntity(TestId? id)
        : base(id) { }

    public static AnotherTestEntity Create()
    {
        return new AnotherTestEntity(TestId.New());
    }

    public static AnotherTestEntity CreateWithId(TestId? testId)
    {
        return new AnotherTestEntity(testId);
    }
}
