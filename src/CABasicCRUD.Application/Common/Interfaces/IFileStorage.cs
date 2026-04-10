namespace CABasicCRUD.Application.Common.Interfaces;

public interface IFileStorage
{
    Task<string> UploadAsync(
        Stream stream,
        string fileName,
        string contentType,
        CancellationToken cancellationToken
    );

    Task DeleteAsync(string fileUrl, CancellationToken cancellationToken);
}
