using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Application.Features.Comments;

public interface ICommentReadService
{
    Task<IReadOnlyList<CommentWithAuthorResult>> GetCommentsWithAuthorOfPostAsync(
        PostId postId,
        CancellationToken cancellationToken
    );
}
