using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Application.Common.Interfaces.Messaging;
using CABasicCRUD.Application.Features.Identity.Auth.Common;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;
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
    IUnitOfWork unitOfWork,
    IMediaRepository mediaRepository
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

        MediaId? oldCoverImageId = user.UserProfile.CoverImageId;

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

        user.UpdateCoverImageId(media.Id);

        if (oldCoverImageId is not null)
        {
            Media? oldMedia = await mediaRepository.GetByIdAsync(oldCoverImageId);

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
