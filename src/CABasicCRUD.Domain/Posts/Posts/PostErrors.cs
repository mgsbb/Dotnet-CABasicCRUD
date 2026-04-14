using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.Posts;

public static class PostErrors
{
    public static readonly Error TitleEmpty = new("Post.Title.Empty", "Post title cannot be empty");

    public static readonly Error ContentEmpty = new(
        "Post.Content.Empty",
        "Post content cannot be empty"
    );

    public static readonly Error MediaItemsLimitReached = new(
        "Posts.MediaItems.LimitReached",
        "Cannot upload more than 5 media items per post."
    );
}
