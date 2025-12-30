using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.UpdateComment;

public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage("Comment Id cannot be empty.");

        RuleFor(x => x.Body).NotEmpty().WithMessage("Comment body cannot be empty.");
    }
}
