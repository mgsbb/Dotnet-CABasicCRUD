using CABasicCRUD.Application.Features.Posts.Comments.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.UpdateComment;

public sealed class UpdateCommentCommandValidator : AbstractValidator<UpdateCommentCommand>
{
    public UpdateCommentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(CommentValidationErrorMessages.IdEmpty);

        RuleFor(x => x.Body).NotEmpty().WithMessage(CommentValidationErrorMessages.BodyEmpty);
    }
}
