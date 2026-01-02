using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.DeletePost;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.DeletePost;

public sealed class DeletePostCommandValidatorTests
{
    private readonly DeletePostCommandValidator _validator;

    public DeletePostCommandValidatorTests()
    {
        _validator = new DeletePostCommandValidator();
    }

    [Fact]
    public void Validate_WhenInputIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        DeletePostCommand command = new(PostId.New());

        // Act
        var result = _validator.Validate(command);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenPostIdIsNull_ShouldHaveValidationError()
    {
        // Arrange
        DeletePostCommand command = new(null!);

        // Act
        var result = _validator.Validate(command);

        // Assert

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PostId");

        // better to avoid testing error messages?
        Assert.Equal(PostValidationErrorMessages.IdEmpty, result.Errors[0].ErrorMessage);
    }
}
