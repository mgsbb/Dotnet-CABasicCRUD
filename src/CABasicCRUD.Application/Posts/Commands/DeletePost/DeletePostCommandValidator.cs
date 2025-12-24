using FluentValidation;

namespace CABasicCRUD.Application.Posts.Commands.DeletePost;

public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post Id cannot be empty");
    }
}
