using CABasicCRUD.Application.Features.Posts;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.CreateComment;

public sealed class CreateCommentCommandValidator : AbstractValidator<CreateCommentCommand>
{
    public CreateCommentCommandValidator()
    {
        RuleFor(x => x.Body).NotEmpty().WithMessage(CommentValidationErrorMessages.BodyEmpty);

        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
