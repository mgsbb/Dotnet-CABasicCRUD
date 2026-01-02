using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.GetPostById;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.GetPostById;

public sealed class GetPostByIdQueryValidatorTests
{
    private readonly GetPostByIdQueryValidator _validator;

    public GetPostByIdQueryValidatorTests()
    {
        _validator = new GetPostByIdQueryValidator();
    }

    [Fact]
    public void Validate_WhenInputIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        GetPostByIdQuery query = new(PostId.New());

        // Act
        var result = _validator.Validate(query);

        // Assert
        Assert.True(result.IsValid);
        Assert.Empty(result.Errors);
    }

    [Fact]
    public void Validate_WhenPostIdIsNull_ShouldHaveValidationError()
    {
        // Arrange
        GetPostByIdQuery query = new(null!);

        // Act
        var result = _validator.Validate(query);

        // Assert

        Assert.False(result.IsValid);
        Assert.Contains(result.Errors, e => e.PropertyName == "PostId");

        // better to avoid testing error messages?
        Assert.Equal(PostValidationErrorMessages.IdEmpty, result.Errors[0].ErrorMessage);
    }
}
