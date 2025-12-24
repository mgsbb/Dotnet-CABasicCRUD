using FluentValidation;

namespace CABasicCRUD.Application.Features.Users.DeleteUser;

public sealed class DeleteUserCommandValidator : AbstractValidator<DeleteUserCommand>
{
    public DeleteUserCommandValidator()
    {
        RuleFor(x => x.UserId).NotEmpty().WithMessage("User Id cannot be empty");
    }
}
