using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands;

public sealed record UpdateUserEmailCommand(UserId UserId, string Email) : ICommand;

internal sealed class UpdateUserEmailCommandHandler(
    IUserReadService userReadService,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<UpdateUserEmailCommand>
{
    public async Task<Result> Handle(
        UpdateUserEmailCommand request,
        CancellationToken cancellationToken
    )
    {
        if (currentUser.UserId != request.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        User? existingUser = await userReadService.GetByEmailAsync(request.Email);

        if (existingUser is not null)
        {
            return Result.Failure(AuthErrors.AlreadyExistsEmail);
        }

        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure(Common.UserErrors.NotFound);
        }

        user.UpdateUserEmail(request.Email);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

public sealed class UpdateUserEmailCommandValidator : AbstractValidator<UpdateUserEmailCommand>
{
    public UpdateUserEmailCommandValidator()
    {
        RuleFor(x => x.Email)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.EmailEmpty)
            .EmailAddress();
    }
}
