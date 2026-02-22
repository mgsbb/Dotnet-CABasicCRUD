using CABasicCRUD.Application.Features.Identity.Users.Common;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage(UserValidationErrorMessages.IdEmpty);
    }
}
