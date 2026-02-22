using CABasicCRUD.Application.Features.Identity.Users.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Identity.Users.Queries.GetUserById;

public sealed class GetUserByIdQueryValidator : AbstractValidator<GetUserByIdQuery>
{
    public GetUserByIdQueryValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(UserValidationErrorMessages.IdEmpty);
    }
}
