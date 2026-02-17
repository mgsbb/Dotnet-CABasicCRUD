using CABasicCRUD.Domain.Posts;
using CABasicCRUD.Domain.Users;
using SortDirection = CABasicCRUD.Domain.Posts.SortDirection;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

public class PostRepository(ApplicationDbContext dbContext)
    : RepositoryBase<Post, PostId>(dbContext),
        IPostRepository
{
    public Task<IReadOnlyList<Post>> SearchPostsAsync(
        string searchTerm,
        int page,
        int pageSize,
        PostOrderBy orderBy,
        SortDirection sortDirection,
        UserId? userId,
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
