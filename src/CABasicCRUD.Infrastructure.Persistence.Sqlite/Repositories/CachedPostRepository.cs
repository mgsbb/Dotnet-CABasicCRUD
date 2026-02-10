namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

using System.Collections.Generic;
using System.Threading.Tasks;
using CABasicCRUD.Domain.Posts;
using Microsoft.Extensions.Caching.Memory;

// NOT USED
public sealed class CachedPostRepository(
    RepositoryBase<Post, PostId> repositoryBase,
    IMemoryCache memoryCache
) : CachedRepositoryBase<Post, PostId>(repositoryBase, memoryCache), IPostRepository
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
