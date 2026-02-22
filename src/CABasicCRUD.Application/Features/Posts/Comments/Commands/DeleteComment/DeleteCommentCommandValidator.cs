using CABasicCRUD.Application.Features.Posts.Comments.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.DeleteComment;

public sealed class DeleteCommentCommandValidator : AbstractValidator<DeleteCommentCommand>
{
    public DeleteCommentCommandValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(CommentValidationErrorMessages.IdEmpty);
    }
}
