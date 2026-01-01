using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.GetCommentById;

public sealed class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(CommentValidationErrorMessages.IdEmpty);
    }
}
