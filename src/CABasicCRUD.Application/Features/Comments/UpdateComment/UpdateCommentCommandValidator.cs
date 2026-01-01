using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.UpdateComment;

public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(CommentValidationErrorMessages.IdEmpty);

        RuleFor(x => x.Body).NotEmpty().WithMessage(CommentValidationErrorMessages.BodyEmpty);
    }
}
