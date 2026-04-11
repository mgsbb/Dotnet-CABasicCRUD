using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.Posts.PostMedias;

namespace CABasicCRUD.Domain.Posts.Posts;

public class Post : AggregateRoot<PostId>
{
    private readonly List<PostMedia> _media = [];

    public string Title { get; private set; }
    public string Content { get; private set; }
    public UserId UserId { get; private set; }
    public IReadOnlyList<PostMedia> Media => _media.AsReadOnly();

    private Post(PostId id, string title, string content, UserId userId)
        : base(id)
    {
        Title = title;
        Content = content;
        UserId = userId;
    }

    public static Result<Post> Create(string? title, string? content, UserId userId)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Result<Post>.Failure(PostErrors.TitleEmpty);
        }
        if (string.IsNullOrEmpty(content))
        {
            return Result<Post>.Failure(PostErrors.ContentEmpty);
        }
        Post post = new(PostId.New(), title, content, userId);
        return post;
    }

    public Result<Post> UpdateTitle(string title)
    {
        if (string.IsNullOrEmpty(title))
        {
            return Result<Post>.Failure(PostErrors.TitleEmpty);
        }
        Title = title;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Result<Post> UpdateContent(string content)
    {
        if (string.IsNullOrEmpty(content))
        {
            return Result<Post>.Failure(PostErrors.ContentEmpty);
        }
        Content = content;
        UpdatedAt = DateTime.UtcNow;
        return this;
    }

    public Result<Post> AddMedia(string url, string? previewUrl, MediaType mediaType)
    {
        if (_media.Count >= 5)
        {
            return Result<Post>.Failure(PostMediaErrors.LimitReached);
        }

        if (mediaType == MediaType.Video && _media.Any(m => m.MediaType == MediaType.Video))
        {
            return Result<Post>.Failure(PostMediaErrors.VideoLimitReached);
        }

        Result<PostMedia> result = PostMedia.Create(url, previewUrl, mediaType);

        if (result.IsFailure)
        {
            return Result<Post>.Failure(result.Error);
        }

        _media.Add(result.Value);

        return this;
    }

    public Result<Post> RemoveMedia(string url)
    {
        PostMedia? mediaToRemove = _media.FirstOrDefault(m => m.Url == url);

        if (mediaToRemove is null)
        {
            return Result<Post>.Failure(PostMediaErrors.NotFound);
        }

        _media.Remove(mediaToRemove);

        return this;
    }
}
