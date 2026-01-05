using System.Text.Json;
using CABasicCRUD.Application.Common.Interfaces;
using CABasicCRUD.Domain.Common;
using CABasicCRUD.Infrastructure.Persistence.Sqlite.Outbox;

namespace CABasicCRUD.Infrastructure.Persistence.Sqlite;

internal sealed class UnitOfWork(ApplicationDbContext dbContext) : IUnitOfWork
{
    private readonly ApplicationDbContext _dbContext = dbContext;

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken)
    {
        var domainEvents = _dbContext
            .ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count > 0)
        {
            var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                // Type = domainEvent.GetType().AssemblyQualifiedName,
                Type = domainEvent.GetType().Name,
                Content = JsonSerializer.Serialize(domainEvent, domainEvent.GetType()),
            });

            _dbContext.AddRange(outboxMessages);

            _dbContext
                .ChangeTracker.Entries<IHasDomainEvents>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());
        }

        return await _dbContext.SaveChangesAsync(cancellationToken);
    }
}
