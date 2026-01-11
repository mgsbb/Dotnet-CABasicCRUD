using CABasicCRUD.Domain.Common;
using Microsoft.Extensions.Caching.Memory;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite.Repositories;

// NOT USED
public class CachedRepositoryBase<TEntity, TId>(
    RepositoryBase<TEntity, TId> repositoryBase,
    IMemoryCache memoryCache
) : IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : EntityIdBase
{
    private readonly RepositoryBase<TEntity, TId> _repositoryBase = repositoryBase;
    private readonly IMemoryCache _memoryCache = memoryCache;

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        return await _repositoryBase.AddAsync(entity);
    }

    public Task DeleteAsync(TEntity entity)
    {
        return _repositoryBase.DeleteAsync(entity);
    }

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        return await _repositoryBase.GetAllAsync();
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        string key = $"{typeof(TEntity)}-{id.Value}";

        /*
        1. Create entity (for eg. create post)
        2. Get entity (get post by id) - entity returned from db and cached
        3. Get entity again - entity returned directly from cache
        4. Update entity (for eg. in update post handler, the entity is fetched from cache to check for existence)
        5. Entity (post) is updated, but since the update is performed on the cached result, the cache is also modified.
        6. Get entity again - modified entity is returned from the cache
        7. Cache is mutable, which causes unintended side effects.
        8. If for some reason, save changes fails for updating the entity (post), then the cache result would be modified, with no changes effected in the db
        */
        TEntity? entity = await _memoryCache.GetOrCreateAsync(
            key,
            entry =>
            {
                entry.SetAbsoluteExpiration(TimeSpan.FromMinutes(10));

                return _repositoryBase.GetByIdAsync(id);
            }
        );

        Console.WriteLine("cache");

        return entity;
    }

    public Task UpdateAsync(TEntity entity)
    {
        return _repositoryBase.UpdateAsync(entity);
    }
}
