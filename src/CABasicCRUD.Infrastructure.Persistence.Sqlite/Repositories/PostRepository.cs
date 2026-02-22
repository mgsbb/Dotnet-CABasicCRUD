using CABasicCRUD.Domain.Posts.Posts;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class PostRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Post, PostId>(dbContext),
        IPostRepository { }
