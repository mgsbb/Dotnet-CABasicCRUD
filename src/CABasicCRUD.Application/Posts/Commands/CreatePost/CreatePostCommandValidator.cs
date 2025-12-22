using FluentValidation;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

internal sealed class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
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
    }
}
