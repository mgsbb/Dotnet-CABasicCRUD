using CABasicCRUD.Application.Common.Interfaces;
using Microsoft.AspNetCore.Hosting;

namespace CABasicCRUD.Infrastructure.Storage;

public sealed class LocalFileStorage : IFileStorage
{
    private readonly string _rootPath;

    public LocalFileStorage(IWebHostEnvironment env)
    {
        _rootPath = Path.Combine(env.ContentRootPath, "uploads", "profile-images");
    }

    public Task DeleteAsync(string fileKey, CancellationToken cancellationToken)
    {
        throw new NotImplementedException();
    }

    public async Task<UploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken
    )
    {
        Directory.CreateDirectory(_rootPath);

        string filePath = Path.Combine(_rootPath, fileName);

        using var fileStream = new FileStream(filePath, FileMode.Create);

        await stream.CopyToAsync(fileStream, cancellationToken);

        return new UploadResult(
            Url: $"uploads/profile-images/{fileName}",
            Key: $"uploads/profile-images/{fileName}"
        );
    }
}
