using CABasicCRUD.Domain.Posts.Comments;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class CommentRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Comment, CommentId>(dbContext),
        ICommentRepository { }
