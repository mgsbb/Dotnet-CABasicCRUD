using FluentValidation;

namespace CABasicCRUD.Application.Features.Users.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be empty");
    }
}
