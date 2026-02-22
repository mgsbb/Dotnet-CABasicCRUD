using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Comments.Queries.GetAllCommentsOfPost;

public sealed class GetAllCommentsOfPostQueryValidator
    : AbstractValidator<GetAllCommentsOfPostQuery>
{
    public GetAllCommentsOfPostQueryValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
