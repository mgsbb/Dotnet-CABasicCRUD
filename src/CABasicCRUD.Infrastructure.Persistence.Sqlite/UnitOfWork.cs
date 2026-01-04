using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite;

internal sealed class UnitOfWork(ApplicationDbContext dbContext, IDomainEventDispatcher dispatcher)
    : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;
    private readonly IDomainEventDispatcher _dispatcher = dispatcher;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        int result = await _dbContext.SaveChangesAsync(cancellationToken);

        var domainEvents = _dbContext
            .ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count > 0)
        {
            await _dispatcher.DispatchAsync(domainEvents, cancellationToken);

            _dbContext
                .ChangeTracker.Entries<IHasDomainEvents>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());
        }

        return result;
    }
}
