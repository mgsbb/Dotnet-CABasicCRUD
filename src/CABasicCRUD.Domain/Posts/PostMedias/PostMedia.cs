using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.PostMedias;

public sealed class PostMedia : EntityBase<PostMediaId>
{
    public string Url { get; private set; }
    public string? PreviewUrl { get; private set; }
    public MediaType MediaType { get; private set; }

    private PostMedia(PostMediaId id, string url, string? previewUrl, MediaType mediaType)
        : base(id)
    {
        Url = url;
        PreviewUrl = previewUrl;
        MediaType = mediaType;
    }

    public static Result<PostMedia> Create(string url, string? previewUrl, MediaType mediaType)
    {
        PostMedia postMedia = new(PostMediaId.New(), url, previewUrl, mediaType);
        return postMedia;
    }
}
