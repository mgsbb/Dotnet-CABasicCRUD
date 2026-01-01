using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.CreatePost;

public sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(PostValidationErrorMessages.TitleEmpty)
            .MaximumLength(100)
            .WithMessage(PostValidationErrorMessages.TitleExceedsMaxCharacters)
            .OverridePropertyName("Title");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(PostValidationErrorMessages.ContentEmpty)
            .OverridePropertyName("Content");
    }
}
