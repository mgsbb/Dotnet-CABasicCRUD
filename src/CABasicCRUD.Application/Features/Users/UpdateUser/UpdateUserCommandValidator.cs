using FluentValidation;

namespace CABasicCRUD.Application.Features.Users.UpdateUser;

public sealed class UpdateUserCommandValidator : AbstractValidator<UpdateUserCommand>
{
    public UpdateUserCommandValidator()
    {
        RuleFor(x => x.Name)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.NameEmpty)
            .MaximumLength(50)
            .WithMessage(UserValidationErrorMessages.NameExceedsMaxCharacters);

        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.EmailEmpty)
            .EmailAddress();

        RuleFor(x => x.UserId).NotEmpty().WithMessage(UserValidationErrorMessages.IdEmpty);
    }
}
