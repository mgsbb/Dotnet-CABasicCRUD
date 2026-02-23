using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class CommentRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Comment, CommentId>(dbContext),
        ICommentRepository { }
