using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.PostMedias;

public static class PostMediaErrors
{
    public static readonly Error LimitReached = new(
        "PostMedia.LimitsReached",
        "Cannot add more than 5 media content per post."
    );
    public static readonly Error NotFound = new("PostMedia.NotFound", "Media not found in post.");

    public static readonly Error VideoLimitReached = new(
        "PostMedia.VideoLimitReached",
        "Cannot add more than 1 video per post."
    );
}
