using FluentValidation;

namespace CABasicCRUD.Application.Posts.Queries.GetPostById;

public sealed class GetPostByIdQueryValidator : AbstractValidator<GetPostByIdQuery>
{
    public GetPostByIdQueryValidator()
    {
        RuleFor(x => x.PostId).NotEmpty().WithMessage("Post Id cannot be empty");
    }
}
