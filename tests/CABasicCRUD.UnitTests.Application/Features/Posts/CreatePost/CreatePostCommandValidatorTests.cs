using CABasicCRUD.Application.Features.Posts;
using CABasicCRUD.Application.Features.Posts.CreatePost;
using FluentValidation.TestHelper;

namespace CABasicCRUD.UnitTests.Application.Features.Posts.CreatePost;

public sealed class CreatePostCommandValidatorTests
{
    private readonly CreatePostCommandValidator _validator;

    public CreatePostCommandValidatorTests()
    {
        _validator = new CreatePostCommandValidator();
    }

    [Fact]
    public void Validate_WhenInputIsValid_ShouldNotHaveValidationError()
    {
        // Arrange
        CreatePostCommand command = new("title", "content");

        // Act
        var result = _validator.TestValidate(command);

        // Assert
        result.ShouldNotHaveAnyValidationErrors();
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
        CreatePostCommand command = new(title!, "content");

        // Act
        var result = _validator.TestValidate(command);

        // Assert

        result
            .ShouldHaveValidationErrorFor(x => x.Title)
            // better to avoid testing error messages?
            .WithErrorMessage(PostValidationErrorMessages.TitleEmpty);
    }

    [Fact]
    public void Validate_WhenTitleExceedsMaxLength_ShouldHaveValidationError()
    {
        // Arrange
        CreatePostCommand command = new(new string('a', 101), "content");

        // Act
        var result = _validator.TestValidate(command);

        // Assert

        result
            .ShouldHaveValidationErrorFor(x => x.Title)
            // better to avoid testing error messages?
            .WithErrorMessage(PostValidationErrorMessages.TitleExceedsMaxCharacters);
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
        CreatePostCommand command = new("title", content!);

        // Act
        var result = _validator.TestValidate(command);

        // Assert

        result
            .ShouldHaveValidationErrorFor(x => x.Content)
            .WithErrorMessage(PostValidationErrorMessages.ContentEmpty);
    }
}
