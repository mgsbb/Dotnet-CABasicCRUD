using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.UpdatePost;

public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
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

        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
