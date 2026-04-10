using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using UserErrors = CABasicCRUD.Application.Features.Identity.Users.Common.UserErrors;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands;

// ========================================================================================================================

public sealed record UpdateUserCoverImageCommand(
    UserId UserId,
    Stream FileStream,
    string FileName,
    string ContentType
) : ICommand;

// ========================================================================================================================

internal sealed class UpdateUserCoverImageCommandHandler(
    ICurrentUser currentUser,
    IFileStorage fileStorage,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork
) : ICommandHandler<UpdateUserCoverImageCommand>
{
    public async Task<Result> Handle(
        UpdateUserCoverImageCommand request,
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
            return Result.Failure(UserErrors.NotFound);
        }

        string? oldCoverImageUrl = user.UserProfile.CoverImageUrl;

        string fileName = $"{user.Id.Value}_{request.FileName}";

        string imageUrl = await fileStorage.UploadAsync(
            request.FileStream,
            fileName,
            request.ContentType,
            cancellationToken
        );

        user.UpdateCoverImageUrl(imageUrl);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        if (!string.IsNullOrWhiteSpace(oldCoverImageUrl))
        {
            await fileStorage.DeleteAsync(oldCoverImageUrl, cancellationToken);
        }

        return Result.Success();
    }
}
