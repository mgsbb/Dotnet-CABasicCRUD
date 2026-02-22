using CABasicCRUD.Domain.Common;
using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Domain.Posts.Comments;

public interface ICommentRepository : IRepository<Comment, CommentId>
{
    Task<IReadOnlyList<Comment>> GetAllCommentsOfPost(PostId postId);
}
