using System.Text.Json;
using CABasicCRUD.Domain.Common;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace CABasicCRUD.Infrastructure.Persistence.PostgreSql.Outbox;

public sealed class OutboxSaveChangesInterceptor(JsonSerializerOptions jsonSerializerOptions)
    : SaveChangesInterceptor
{
    private readonly JsonSerializerOptions _jsonSerializerOptions = jsonSerializerOptions;

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default
    )
    {
        var dbContext = eventData.Context;

        if (dbContext is null)
        {
            return base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        AddOutboxMessages(dbContext);

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void AddOutboxMessages(DbContext dbContext)
    {
        var domainEvents = dbContext
            .ChangeTracker.Entries<IHasDomainEvents>()
            .SelectMany(e => e.Entity.DomainEvents)
            .ToList();

        if (domainEvents.Count > 0)
        {
            var outboxMessages = domainEvents.Select(domainEvent => new OutboxMessage
            {
                Id = Guid.NewGuid(),
                OccurredOnUtc = DateTime.UtcNow,
                Type = domainEvent.GetType().AssemblyQualifiedName!,
                // Type = domainEvent.GetType().Name,
                Content = JsonSerializer.Serialize(
                    domainEvent,
                    domainEvent.GetType(),
                    _jsonSerializerOptions
                ),
            });

            dbContext.Set<OutboxMessage>().AddRange(outboxMessages);

            dbContext
                .ChangeTracker.Entries<IHasDomainEvents>()
                .ToList()
                .ForEach(e => e.Entity.ClearDomainEvents());
        }
    }
}
