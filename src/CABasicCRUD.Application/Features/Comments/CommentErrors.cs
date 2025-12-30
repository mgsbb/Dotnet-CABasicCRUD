using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Features.Comments;

public static class CommentErrors
{
    public static readonly Error NotFound = new(
        "Comment.NotFound",
        "Comment with the given Id is not found."
    );
}
