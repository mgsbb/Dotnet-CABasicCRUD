using CABasicCRUD.Domain.MediaItems;

namespace CABasicCRUD.Domain.Posts.Posts;

public sealed class PostMedia
{
    public PostId PostId { get; private set; }
    public MediaId MediaId { get; private set; }

    internal PostMedia(PostId postId, MediaId mediaId)
    {
        PostId = postId;
        MediaId = mediaId;
    }
}
