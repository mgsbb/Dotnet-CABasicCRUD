using FluentValidation;

namespace CABasicCRUD.Application.Features.Auth.LoginUser;

public sealed class LoginUserCommandValidator : AbstractValidator<LoginUserCommand>
{
    public LoginUserCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AuthValidationErrorMessages.EmailEmpty)
            .EmailAddress();

        RuleFor(x => x.Password).NotEmpty().WithMessage(AuthValidationErrorMessages.PasswordEmpty);
    }
}
