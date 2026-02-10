using CABasicCRUD.Domain.Posts;

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
        CancellationToken cancellationToken
    )
    {
        throw new NotImplementedException();
    }
}
