namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

using CABasicCRUD.Domain.Posts;
using Microsoft.Extensions.Caching.Memory;

// NOT USED
public sealed class CachedPostRepository(
    RepositoryBase<Post, PostId> repositoryBase,
    IMemoryCache memoryCache
) : CachedRepositoryBase<Post, PostId>(repositoryBase, memoryCache), IPostRepository { }
