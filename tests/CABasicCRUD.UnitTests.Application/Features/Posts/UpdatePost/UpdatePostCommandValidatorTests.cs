using CABasicCRUD.Application.Features.Posts.UpdatePost;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.UpdatePost;

public sealed class UpdatePostCommandValidatorTests
{
    private readonly UpdatePostCommandValidator _validator;

    public UpdatePostCommandValidatorTests()
    {
        _validator = new UpdatePostCommandValidator();
    }

    [Fact]
    public void Validate_WhenInputIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        UpdatePostCommand command = new(PostId.New(), "title", "content");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_WhenTitleIsNullOrWhitespace_ShouldHaveValidationError(string? title)
    {
        // Arrange
        UpdatePostCommand command = new(PostId.New(), title!, "content");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Fact]
    public void Validate_WhenTitleExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        UpdatePostCommand command = new(PostId.New(), new string('a', 101), "content");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Title");
    }

    [Theory]
    [InlineData(null)]
    [InlineData("")]
    [InlineData(" ")]
    [InlineData("\t")]
    [InlineData("\n")]
    public void Validate_WhenContentIsNullOrWhitespace_ShouldHaveValidationError(string? content)
    {
        // Arrange
        UpdatePostCommand command = new(PostId.New(), "title", content!);

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "Content");
    }

    [Fact]
    public void Validate_WhenPostIdIsNull_ShouldHaveValidationError()
    {
        // Arrange
        UpdatePostCommand command = new(null!, "title", "content");

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PostId");
    }
}
