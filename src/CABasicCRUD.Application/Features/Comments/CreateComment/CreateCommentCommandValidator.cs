using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Body).NotEmpty().WithMessage("Comment body cannot be empty.");

        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post Id cannot be empty.");
    }
}
