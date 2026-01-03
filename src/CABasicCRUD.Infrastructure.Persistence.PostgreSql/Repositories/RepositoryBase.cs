using CABasicCRUD.Domain.Common;
using Microsoft.EntityFrameworkCore;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Repositories;

public class RepositoryBase<TEntity, TId>(ApplicationDbContext dbContext)
    : IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : EntityIdBase
{
    protected readonly ApplicationDbContext _dbContext = dbContext;
    protected readonly DbSet<TEntity> _dbSet = dbContext.Set<TEntity>();

    public async Task<IReadOnlyList<TEntity>> GetAllAsync()
    {
        IReadOnlyList<TEntity> entities = await _dbSet.AsNoTracking().ToListAsync();

        return entities;
    }

    public async Task<TEntity?> GetByIdAsync(TId id)
    {
        TEntity? entity = await _dbSet
            .AsNoTracking()
            .FirstOrDefaultAsync(entity => entity.Id == id);

        return entity;
    }

    public async Task<TEntity> AddAsync(TEntity entity)
    {
        await _dbSet.AddAsync(entity);

        return entity;
    }

    public Task UpdateAsync(TEntity entity)
    {
        _dbSet.Update(entity);

        return Task.CompletedTask;
    }

    public Task DeleteAsync(TEntity entity)
    {
        _dbSet.Remove(entity);

        return Task.CompletedTask;
    }
}
