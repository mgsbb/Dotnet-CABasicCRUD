using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.UpdatePost;

public sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.Title is not null || x.Content is not null)
            .OverridePropertyName("General")
            .WithMessage("At least one field must be provided.");

        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage(PostValidationErrorMessages.TitleEmpty)
            .MaximumLength(100)
            .WithMessage(PostValidationErrorMessages.TitleExceedsMaxCharacters)
            .OverridePropertyName("Title")
            .When(x => x.Title is not null);

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage(PostValidationErrorMessages.ContentEmpty)
            .OverridePropertyName("Content")
            .When(x => x.Content is not null);

        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
