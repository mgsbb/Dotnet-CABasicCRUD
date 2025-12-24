using FluentValidation;

namespace CABasicCRUD.Application.Features.Posts.GetPostById;

public sealed class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post Id cannot be empty");
    }
}
