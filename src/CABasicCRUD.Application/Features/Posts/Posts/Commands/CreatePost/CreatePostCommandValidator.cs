using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.CreatePost;

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
