using FluentValidation;

namespace CABasicCRUD.Application.Features.Auth.RegisterUser;

public sealed class RegisterUserCommandValidator : AbstractValidator<RegisterUserCommand>
{
    public RegisterUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("User name is required.")
            .MaximumLength(50)
            .WithMessage("User name must not exceed 50 characters.");

        RuleFor(x => x.Email).NotEmpty().WithMessage("User email is required.").EmailAddress();

        RuleFor(x => x.Password)
            .NotEmpty()
            .WithMessage("User password is required.")
            .MinimumLength(8)
            .WithMessage("Password must be at least 8 characters long.")
            .MaximumLength(128)
            .WithMessage("Password  must not exceed 128 characters.")
            .Matches(@"[A-Z]")
            .WithMessage("Password must contain at least one uppercase letter.")
            .Matches(@"[a-z]")
            .WithMessage("Password must contain at least one lowercase letter.")
            .Matches(@"\d")
            .WithMessage("Password must contain at least one digit.")
            .Matches(@"[!@#$%^&*(),.?""{}|<>_\-+=]")
            .WithMessage(
                @"Password must contain at least one special character from [!@#$%^&*(),.?""{}|<>_\-+=]."
            )
            .Must(p => p == p.Trim())
            .WithMessage("Password must not start or end with whitespace.");
    }
}
