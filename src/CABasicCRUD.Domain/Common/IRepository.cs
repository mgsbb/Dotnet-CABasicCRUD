namespace CABasicCRUD.Domain.Common;

public interface IRepository<TEntity, TId>
    where TEntity : EntityBase<TId>
    where TId : EntityIdBase
{
    public Task<IReadOnlyList<TEntity>> GetAllAsync();

    public Task<TEntity?> GetByIdAsync(TId id);

    public Task<TEntity> AddAsync(TEntity entity);

    public Task UpdateAsync(TEntity entity);

    public Task DeleteAsync(TEntity entity);
}
