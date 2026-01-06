using CABasicCRUD.Application.Features.Comments;
using CABasicCRUD.Presentation.WebApi.Features.Comments.Contracts;

namespace CABasicCRUD.Presentation.WebApi.Features.Comments;

internal static class CommentMappings
{
    internal static CommentResponse ToCommentResponse(this CommentResult commentResult)
    {
        return new(
            commentResult.Id,
            commentResult.Body,
            commentResult.PostId,
            commentResult.UserId,
            commentResult.CreatedAt,
            commentResult.UpdatedAt
        );
    }

    internal static IReadOnlyList<CommentResponse> ToListCommentResponse(
        this IReadOnlyList<CommentResult> comments
    )
    {
        if (comments == null)
            return new List<CommentResponse>();

        return comments.Select(comment => comment.ToCommentResponse()).ToList();
    }
}
