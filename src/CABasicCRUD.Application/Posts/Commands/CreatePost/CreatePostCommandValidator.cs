using FluentValidation;

namespace CABasicCRUD.Application.Posts.Commands.CreatePost;

public class CreatePostCommandValidator : AbstractValidator<CreatePostCommand>
{
    public CreatePostCommandValidator()
    {
        RuleFor(x => x.CreatePostDTO.Title)
            .NotEmpty()
            .WithMessage("Post title cannot be empty")
            .MaximumLength(100)
            .WithMessage("Post title cannot be more than 100 characters");

        RuleFor(x => x.CreatePostDTO.Content)
            .NotEmpty()
            .WithMessage("Post content cannot be empty");
    }
}
