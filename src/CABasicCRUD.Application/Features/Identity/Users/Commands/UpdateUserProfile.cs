using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Application.Features.Identity.Users.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands;

// ========================================================================================================================

public sealed record UpdateUserProfileCommand(UserId UserId, string? FullName, string? Bio)
    : ICommand;

// ========================================================================================================================

internal sealed class UpdateUserProfileCommandHandler(
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    ICurrentUser currentUser
) : ICommandHandler<UpdateUserProfileCommand>
{
    public async Task<Result> Handle(
        UpdateUserProfileCommand request,
        CancellationToken cancellationToken
    )
    {
        if (currentUser.UserId != request.UserId)
        {
            return Result.Failure(AuthErrors.Forbidden);
        }

        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result.Failure(Common.UserErrors.NotFound);
        }

        user.UpdateUserProfile(request.FullName, request.Bio);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}

// ========================================================================================================================

public sealed class UpdateUserProfileCommandValidator : AbstractValidator<UpdateUserProfileCommand>
{
    public UpdateUserProfileCommandValidator()
    {
        RuleFor(x => x)
            .Must(x => x.FullName is not null || x.Bio is not null)
            .OverridePropertyName("General")
            .WithMessage("At least one field must be provided");

        RuleFor(x => x.FullName)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.NameEmpty)
            .MaximumLength(50)
            .WithMessage(UserValidationErrorMessages.NameExceedsMaxCharacters)
            .When(x => x.FullName is not null);

        RuleFor(x => x.Bio)
            .NotEmpty()
            .WithMessage(UserValidationErrorMessages.BioEmpty)
            .MaximumLength(200)
            .WithMessage(UserValidationErrorMessages.BioExceedsMaxCharacters)
            .When(x => x.Bio is not null);
    }
}

// ========================================================================================================================
