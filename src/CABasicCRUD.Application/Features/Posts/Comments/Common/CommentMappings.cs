using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Application.Features.Posts.Comments.Common;

internal static class CommentMappings
{
    internal static CommentResult ToCommentResult(this Comment comment)
    {
        return new CommentResult(
            comment.Id,
            comment.Body,
            comment.PostId,
            comment.UserId,
            comment.CreatedAt,
            comment.UpdatedAt
        );
    }

    internal static IReadOnlyList<CommentResult> ToListCommentResult(
        this IReadOnlyList<Comment> comments
    )
    {
        if (comments == null)
            return [];

        return [.. comments.Select(comment => comment.ToCommentResult())];
    }
}
