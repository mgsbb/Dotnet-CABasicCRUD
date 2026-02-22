using CABasicCRUD.Application.Features.Posts.Posts.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.Posts.Queries.GetPostById;

public sealed class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage(PostValidationErrorMessages.IdEmpty);
    }
}
