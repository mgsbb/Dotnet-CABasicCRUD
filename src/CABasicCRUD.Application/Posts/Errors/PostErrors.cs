using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Posts.Errors;

public static class PostErrors
{
    public static readonly Error NotFound = new(
        "Post.NotFound",
        "Post with the given Id is not found."
    );
}
