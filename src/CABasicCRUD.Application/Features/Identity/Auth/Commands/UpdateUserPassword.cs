using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using FluentValidation;

namespace CABasicCRUD.Application.Features.Identity.Auth.Commands;

// ========================================================================================================================

public sealed record UpdateUserPasswordCommand(
    UserId UserId,
    string OldPassword,
    string NewPassword,
    string NewPasswordConfirmed
) : ICommand<TokenResult>;

// ========================================================================================================================

public sealed record TokenResult(string Token);

// ========================================================================================================================

internal sealed class UpdateUserPasswordCommandHandler(
    ICurrentUser currentUser,
    IUserRepository userRepository,
    IPasswordHasher passwordHasher,
    IUnitOfWork unitOfWork,
    IJwtProvider jwtProvider
) : ICommandHandler<UpdateUserPasswordCommand, TokenResult>
{
    public async Task<Result<TokenResult>> Handle(
        UpdateUserPasswordCommand request,
        CancellationToken cancellationToken
    )
    {
        if (request.UserId != currentUser.UserId)
        {
            return Result<TokenResult>.Failure(AuthErrors.Forbidden);
        }

        if (request.NewPassword != request.NewPasswordConfirmed)
        {
            return Result<TokenResult>.Failure(AuthErrors.PasswordsMismatch);
        }

        User? user = await userRepository.GetByIdAsync(request.UserId);

        if (user is null)
        {
            return Result<TokenResult>.Failure(Users.Common.UserErrors.NotFound);
        }

        if (!user.VerifyPassword(request.OldPassword, passwordHasher))
        {
            return Result<TokenResult>.Failure(AuthErrors.InvalidPassword);
        }

        user.UpdatePassword(request.NewPassword, passwordHasher);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        string token = jwtProvider.GenerateToken(user);

        return Result<TokenResult>.Success(new TokenResult(token));
    }
}

// ========================================================================================================================

public sealed class UpdateUserPasswordCommandValidator
    : AbstractValidator<UpdateUserPasswordCommand>
{
    public UpdateUserPasswordCommandValidator()
    {
        RuleFor(x => x.NewPassword)
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

// ========================================================================================================================
