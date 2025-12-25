using FluentValidation;

namespace CABasicCRUD.Application.Features.Auth.LoginUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email).NotEmpty().WithMessage("User email is required.").EmailAddress();

        RuleFor(x => x.Password).NotEmpty().WithMessage("User password is required.");
    }
}
