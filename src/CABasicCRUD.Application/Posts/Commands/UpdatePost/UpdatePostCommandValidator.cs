using FluentValidation;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

internal sealed class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .WithMessage("Post title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Post title cannot be more than 100 characters")
            .OverridePropertyName("Title");

        RuleFor(x => x.Content)
            .NotEmpty()
            .WithMessage("Post content cannot be empty")
            .OverridePropertyName("Content");

        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post ID cannot be empty");
    }
}
