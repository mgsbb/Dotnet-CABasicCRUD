using FluentValidation;

namespace CABasicCRUD.Application.Features.Auth.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(AuthValidationErrorMessages.NameEmpty)
            .MaximumLength(50)
            .WithMessage(AuthValidationErrorMessages.NameExceedsMaxCharacters);

        RuleFor(x => x.Username)
            .NotEmpty()
            .WithMessage(AuthValidationErrorMessages.UsernameEmtpy)
            .MinimumLength(5)
            .WithMessage(AuthValidationErrorMessages.UsernameLessThanMinCharacters)
            .MaximumLength(30)
            .WithMessage(AuthValidationErrorMessages.UsernameExceedsMaxCharacters);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(AuthValidationErrorMessages.EmailEmpty)
            .EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage(AuthValidationErrorMessages.PasswordEmpty)
            .MinimumLength(8)
            .WithMessage(AuthValidationErrorMessages.PasswordMinCharacters)
            .MaximumLength(128)
            .WithMessage(AuthValidationErrorMessages.PasswordMaxCharacters)
            .Matches(@"[A-Z]")
            .WithMessage(AuthValidationErrorMessages.PasswordUppercase)
            .Matches(@"[a-z]")
            .WithMessage(AuthValidationErrorMessages.PasswordLowercase)
            .Matches(@"\d")
            .WithMessage(AuthValidationErrorMessages.PasswordDigit)
            .Matches(@"[!@#$%^&*(),.?""{}|<>_\-+=]")
            .WithMessage(AuthValidationErrorMessages.PasswordSpecial)
            .Must(p => p == p.Trim())
            .WithMessage(AuthValidationErrorMessages.PasswordWhitespace);
    }
}
