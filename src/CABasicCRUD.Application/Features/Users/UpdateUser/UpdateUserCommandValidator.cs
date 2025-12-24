using FluentValidation;

namespace CABasicCRUD.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage("User name cannot be empty")
            .MaximumLength(50)
            .WithMessage("User name cannot exceed 50 characters");

        RuleFor(x => x.Email).NotEmpty().WithMessage("User email cannot be empty").EmailAddress();

        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be empty");
    }
}
