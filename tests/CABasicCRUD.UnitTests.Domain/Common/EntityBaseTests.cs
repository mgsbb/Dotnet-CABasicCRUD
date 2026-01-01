namespace CABasicCRUD.UnitTests.Domain.Common;

public sealed class EntityBaseTests
{
    [Fact]
    public void Ctor_Should_ThrowArgumentException_WhenGivenIdIsNull()
    {
        // Arrange
        TestId? nullId = null;

        // Act
        // Assert
        Assert.Throws<ArgumentException>(() => TestEntity.CreateWithId(nullId));
    }

    [Fact]
    public void Ctor_Should_SetId_WhenGivenIdIsValid()
    {
        // Arrange
        TestId validId = TestId.New();

        // Act
        TestEntity testEntity = TestEntity.CreateWithId(validId);

        // Assert
        Assert.Equal(validId, testEntity.Id);
    }

    [Fact]
    public void Equals_Should_ReturnTrue_ForEntitiesWithSameId()
    {
        // Arrange
        TestId testId = TestId.New();
        TestEntity testEntity1 = TestEntity.CreateWithId(testId);
        TestEntity testEntity2 = TestEntity.CreateWithId(testId);

        // Act
        // Assert
        Assert.True(testEntity1.Equals(testEntity2));
        Assert.True(testEntity1.Equals((object)testEntity2));
        Assert.True(testEntity1 == testEntity2);
        Assert.False(testEntity1 != testEntity2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_ForEntitiesWithDifferentId()
    {
        // Arrange
        TestEntity testEntity1 = TestEntity.Create();
        TestEntity testEntity2 = TestEntity.Create();

        // Act
        // Assert
        Assert.False(testEntity1.Equals(testEntity2));
        Assert.False(testEntity1.Equals((object)testEntity2));
        Assert.False(testEntity1 == testEntity2);
        Assert.True(testEntity1 != testEntity2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_WhenOneEntityIsNull()
    {
        // Arrange
        TestEntity testEntity1 = TestEntity.Create();
        TestEntity? testEntity2 = null;

        // Act
        // Assert
        Assert.False(testEntity1.Equals(testEntity2));
        Assert.False(testEntity1.Equals((object?)testEntity2));
        Assert.False(testEntity1 == testEntity2);
        Assert.True(testEntity1 != testEntity2);
    }

    [Fact]
    public void Equals_Should_ReturnTrue_WhenBothEntitiesAreNull()
    {
        // Arrange
        TestEntity? testEntity1 = null;
        TestEntity? testEntity2 = null;

        // Act
        // Assert
        Assert.True(testEntity1 == testEntity2);
        Assert.False(testEntity1 != testEntity2);
    }

    [Fact]
    public void Equals_Should_ReturnTrue_WhenBothEntitiesAreSameReference()
    {
        // Arrange
        TestEntity testEntity1 = TestEntity.Create();
        TestEntity testEntity2 = testEntity1;

        // Act
        // Assert
        Assert.True(testEntity1.Equals(testEntity2));
        Assert.True(testEntity1.Equals((object?)testEntity2));
        Assert.True(testEntity1 == testEntity2);
        Assert.False(testEntity1 != testEntity2);
    }

    [Fact]
    public void Equals_Should_ReturnFalse_WhenEntitiesAreOfDifferentTypesWithSameId()
    {
        // Arrange
        TestId testId = TestId.New();
        TestEntity testEntity = TestEntity.CreateWithId(testId);
        AnotherTestEntity anotherTestEntity = AnotherTestEntity.CreateWithId(testId);
        ;

        // Act
        // Assert
        Assert.False(testEntity.Equals(anotherTestEntity));
        Assert.False(testEntity.Equals((object?)anotherTestEntity));
        Assert.False(testEntity == anotherTestEntity);
        Assert.True(testEntity != anotherTestEntity);
    }

    [Fact]
    public void GetHashCode_Should_ReturnSameHashCode_WhenEntitiesAreWithSameId()
    {
        // Arrange
        TestId testId = TestId.New();
        TestEntity testEntity1 = TestEntity.CreateWithId(testId);
        TestEntity testEntity2 = TestEntity.CreateWithId(testId);
        ;

        // Act
        // Assert
        Assert.Equal(testEntity1.GetHashCode(), testEntity2.GetHashCode());
        Assert.Equal(testEntity1.GetHashCode(), testId.Value.GetHashCode());
    }

    [Fact]
    public void GetHashCode_Should_ReturnDifferentHashCode_WhenEntitiesAreWithDifferentId()
    {
        // Arrange
        TestEntity testEntity1 = TestEntity.Create();
        TestEntity testEntity2 = TestEntity.Create();
        ;

        // Act
        // Assert
        Assert.NotEqual(testEntity1.GetHashCode(), testEntity2.GetHashCode());
    }
}


// Arrange

// Act

// Assert
