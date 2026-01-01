namespace CABasicCRUD.UnitTests.Domain.Common;

public sealed class EntityIdBaseTests
{
    [Fact]
    public void ExplicitConversion_Should_ReturnCorrectId_WhenGuidIsValid()
    {
        // Arrange
        Guid validGuid = Guid.NewGuid();

        // Act
        TestId testId = (TestId)validGuid;

        // Assert
        Assert.NotNull(testId);
        Assert.Equal(testId.Value, validGuid);
    }

    [Fact]
    public void Ctor_Should_ThrowArgumentException_WhenGuidIsEmpty()
    {
        // Arrange
        Guid emptyGuid = Guid.Empty;

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => (TestId)emptyGuid);
    }

    [Fact]
    public void Ctor_Should_NotThrowExceptionAndSetCorrectValue_WhenGuidIsValid()
    {
        // Arrange
        Guid validGuid = Guid.NewGuid();

        // Act
        TestId testId = (TestId)validGuid;

        // Assert
        Assert.Equal(validGuid, testId.Value);
    }

    [Fact]
    public void ImplicitConversion_Should_ReturnCorrectGuid()
    {
        // Arrange
        Guid guid = Guid.NewGuid();
        TestId testId = (TestId)guid;

        // Act
        Guid convertedGuid = testId;

        // Assert
        Assert.Equal(guid, convertedGuid);
    }

    [Fact]
    public void New_Should_ReturnValidId()
    {
        // Arrange
        TestId testId = TestId.New();

        // Act
        // Assert
        Assert.NotNull(testId);
        Assert.NotEqual(testId.Value, Guid.Empty);
    }

    [Fact]
    public void New_TwoConsecutiveCalls_Should_ReturnDifferentIds()
    {
        // Arrange
        TestId testId1 = TestId.New();
        TestId testId2 = TestId.New();

        // Act
        // Assert
        Assert.False(ReferenceEquals(testId1, testId2));
        Assert.NotEqual(testId1, testId2);
        Assert.NotEqual(testId1.Value, testId2.Value);
        Assert.True(testId1 != testId2);
        Assert.False(testId1.Equals(testId2));
    }
}
