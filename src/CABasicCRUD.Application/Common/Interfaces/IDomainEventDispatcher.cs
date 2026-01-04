using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IDomainEventDispatcher
{
    Task DispatchAsync(
        IReadOnlyList<IDomainEvent> domainEvents,
        CancellationToken cancellationToken
    );
}
