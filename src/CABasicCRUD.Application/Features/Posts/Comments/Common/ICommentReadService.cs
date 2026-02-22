using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Application.Features.Posts.Comments.Common;

public interface ICommentReadService
{
    Task<IReadOnlyList<CommentWithAuthorResult>> GetCommentsWithAuthorOfPostAsync(
        PostId postId,
        CancellationToken cancellationToken
    );
}
