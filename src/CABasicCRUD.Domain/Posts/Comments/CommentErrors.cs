using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.Comments;

public static class CommentErrors
{
    public static readonly Error BodyEmpty = new(
        "Comment.Body.Empty",
        "Comment body cannot be empty"
    );
}
