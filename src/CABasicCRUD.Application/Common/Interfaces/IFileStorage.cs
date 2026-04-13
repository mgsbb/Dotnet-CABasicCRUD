namespace CABasicCRUD.Application.Common.Interfaces;

public interface IFileStorage
{
    Task<UploadResult> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken
    );

    Task DeleteAsync(string fileKey, CancellationToken cancellationToken);
}

public record UploadResult(string Key, string Url);
