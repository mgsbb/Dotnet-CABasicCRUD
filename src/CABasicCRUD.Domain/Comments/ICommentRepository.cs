using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts;

namespace CABasicCRUD.Domain.Comments;

public interface ICommentRepository : IRepository<Comment, CommentId>
{
    Task<IReadOnlyList<Comment>> GetAllCommentsOfPost(PostId postId);
}
