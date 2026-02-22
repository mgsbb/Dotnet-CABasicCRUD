using CABasicCRUD.Application.Features.Posts.Comments.Common;
using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Comments.Commands.CreateComment;

public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Body).NotEmpty().WithMessage(CommentValidationErrorMessages.BodyEmpty);

        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
