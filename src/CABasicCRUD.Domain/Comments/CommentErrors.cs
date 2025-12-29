using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Comments;

public static class CommentErrors
{
    public static readonly Error BodyEmpty = new(
        "Comment.Body.Empty",
        "Comment body cannot be empty"
    );
}
