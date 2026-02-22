using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Posts.Commands.DeletePost;

public sealed class DeletePostCommandValidator : AbstractValidator<DeletePostCommand>
{
    public DeletePostCommandValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
