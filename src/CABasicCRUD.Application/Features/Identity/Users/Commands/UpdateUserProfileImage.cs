using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;
using UserErrors = CABasicCRUD.Application.Features.Identity.Users.Common.UserErrors;

namespace CABasicCRUD.Application.Features.Identity.Users.Commands;

// ========================================================================================================================

public sealed record UpdateUserProfileImageCommand(
    UserId UserId,
    Stream FileStream,
    string FileName,
    string ContentType
) : ICommand;

// ========================================================================================================================

internal sealed class UpdateUserProfileImageCommandHandler(
    ICurrentUser currentUser,
    IFileStorage fileStorage,
    IUserRepository userRepository,
    IUnitOfWork unitOfWork,
    IMediaRepository mediaRepository
) : ICommandHandler<UpdateUserProfileImageCommand>
{
    public async Task<Result> Handle(
        UpdateUserProfileImageCommand request,
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

        MediaId? oldProfileImageId = user.UserProfile.ProfileImageId;

        string fileName = $"{user.Id.Value}_{request.FileName}";

        UploadResult result = await fileStorage.UploadAsync(
            request.FileStream,
            fileName,
            request.ContentType,
            cancellationToken
        );

        Media media = Media.Create(
            StorageProvider.Cloudinary,
            result.Key,
            result.Url,
            MediaType.Image,
            request.FileName,
            request.FileStream.Length,
            request.ContentType
        );

        await mediaRepository.AddAsync(media);

        user.UpdateProfileImageId(media.Id);

        if (oldProfileImageId is not null)
        {
            Media? oldMedia = await mediaRepository.GetByIdAsync(oldProfileImageId);

            if (oldMedia is not null)
            {
                await fileStorage.DeleteAsync(oldMedia.StorageKey, cancellationToken);
                await mediaRepository.DeleteAsync(oldMedia);
            }
        }
        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Result.Success();
    }
}
