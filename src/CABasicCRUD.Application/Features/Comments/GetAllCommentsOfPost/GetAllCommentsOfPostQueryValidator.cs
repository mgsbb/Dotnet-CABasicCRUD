using CABasicCRUD.Application.Features.Posts;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Comments.GetAllCommentsOfPost;

public sealed class GetAllCommentsOfPostQueryValidator
    : AbstractValidator<GetAllCommentsOfPostQuery>
{
    public GetAllCommentsOfPostQueryValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
