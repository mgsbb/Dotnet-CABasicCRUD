using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.UnitTests.Domain.Common;

public class TestEntity : EntityBase<TestId>
{
    private TestEntity(TestId? id)
        : base(id) { }

    public static TestEntity Create()
    {
        return new TestEntity(TestId.New());
    }

    public static TestEntity CreateWithId(TestId? testId)
    {
        return new TestEntity(testId);
    }
}
