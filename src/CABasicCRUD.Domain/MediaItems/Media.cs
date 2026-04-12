using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.MediaItems;

public sealed class Media : EntityBase<MediaId>
{
    public StorageProvider StorageProvider { get; private set; }
    public string StorageKey { get; private set; }
    public string Url { get; private set; }
    public MediaType MediaType { get; private set; }

    // metadata
    public string FileName { get; private set; }
    public long Size { get; private set; }
    public string? ContentType { get; private set; }

    private Media(
        MediaId id,
        StorageProvider storageProvider,
        string storageKey,
        string url,
        MediaType mediaType,
        string fileName,
        long size,
        string? contentType
    )
        : base(id)
    {
        StorageProvider = storageProvider;
        StorageKey = storageKey;
        Url = url;
        MediaType = mediaType;
        FileName = fileName;
        Size = size;
        ContentType = contentType;
    }

    public static Media Create(
        StorageProvider storageProvider,
        string storageKey,
        string url,
        MediaType mediaType,
        string fileName,
        long size,
        string? contentType
    )
    {
        return new(
            MediaId.New(),
            storageProvider,
            storageKey,
            url,
            mediaType,
            fileName,
            size,
            contentType
        );
    }
}
