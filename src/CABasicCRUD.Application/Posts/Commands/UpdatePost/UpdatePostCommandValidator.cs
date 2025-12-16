using FluentValidation;

namespace CABasicCRUD.Application.Posts.Commands.UpdatePost;

public class UpdatePostCommandValidator : AbstractValidator<UpdatePostCommand>
{
    public UpdatePostCommandValidator()
    {
        RuleFor(x => x.UpdatePostDTO.Title)
            .NotEmpty()
            .WithMessage("Post title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Post title cannot be more than 100 characters")
            .OverridePropertyName("Title");

        RuleFor(x => x.UpdatePostDTO.Content)
            .NotEmpty()
            .WithMessage("Post content cannot be empty")
            .OverridePropertyName("Content");

        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post ID cannot be empty");
    }
}
