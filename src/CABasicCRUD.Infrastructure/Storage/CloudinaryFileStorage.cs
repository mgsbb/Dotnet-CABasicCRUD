using CABasicCRUD.Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;
using UploadResult = CABasicCRUD.Application.Common.Interfaces.UploadResult;

namespace CABasicCRUD.Infrastructure.Storage;

public sealed class CloudinaryFileStorage(Cloudinary cloudinary) : IFileStorage
{
    public async Task<UploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken
    )
    {
        var uploadParams = new AutoUploadParams { File = new FileDescription(fileName, stream) };

        var result = await cloudinary.UploadAsync(uploadParams, cancellationToken);

        return new(result.PublicId, result.SecureUrl.ToString());
    }

    public async Task DeleteAsync(string fileKey, CancellationToken cancellationToken)
    {
        var deleteParams = new DeletionParams(fileKey);

        await cloudinary.DestroyAsync(deleteParams);
    }
}
