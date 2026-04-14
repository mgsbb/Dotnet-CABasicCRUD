using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Identity.Users;
using CABasicCRUD.Domain.MediaItems;

namespace CABasicCRUD.Domain.Posts.Posts;

public class Post : AggregateRoot<PostId>
{
    public string Title { get; private set; }
    public string Content { get; private set; }
    public UserId UserId { get; private set; }

    private List<PostMedia> _postMediaItems = [];

    // store MediaId, and not Media, due to aggregate boundaries
    // drawbacks - can't immediately know details of media, such as if Video or Image
    public IReadOnlyList<PostMedia> PostMediaItems => _postMediaItems.AsReadOnly();

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

    public Result<Post> AddMedia(MediaId mediaId)
    {
        if (_postMediaItems.Count >= 5)
        {
            return Result<Post>.Failure(PostErrors.MediaItemsLimitReached);
        }

        // can't check if there is atleast 1 video already existing, as only the MediaId is stored
        // perform the check in the application layer

        _postMediaItems.Add(new PostMedia(Id, mediaId));

        return this;
    }

    public Result<Post> RemoveMedia(MediaId mediaId)
    {
        PostMedia? media = _postMediaItems.FirstOrDefault(pm => pm.MediaId == mediaId);

        if (media is not null)
        {
            _postMediaItems.Remove(media);
        }

        return this;
    }
}
