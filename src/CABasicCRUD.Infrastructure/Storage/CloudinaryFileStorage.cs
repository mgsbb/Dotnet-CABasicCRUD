using CABasicCRUD.Application.Common.Interfaces;
using CloudinaryDotNet;
using CloudinaryDotNet.Actions;

namespace CABasicCRUD.Infrastructure.Storage;

public sealed class CloudinaryFileStorage(Cloudinary cloudinary) : IFileStorage
{
    public async Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken
    )
    {
        var uploadParams = new ImageUploadParams { File = new FileDescription(fileName, stream) };

        var result = await cloudinary.UploadAsync(uploadParams, cancellationToken);

        return result.SecureUrl.ToString();
    }
}
