using CABasicCRUD.Application.Features.Posts.Comments.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetCommentById;

public sealed class GetCommentByIdQueryValidator : AbstractValidator<GetCommentByIdQuery>
{
    public GetCommentByIdQueryValidator()
    {
        RuleFor(x => x.Id).NotEmpty().WithMessage(CommentValidationErrorMessages.IdEmpty);
    }
}
