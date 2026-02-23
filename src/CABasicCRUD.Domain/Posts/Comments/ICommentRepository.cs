using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Domain.Posts.Comments;

public interface ICommentRepository : IRepository<Comment, CommentId> { }
