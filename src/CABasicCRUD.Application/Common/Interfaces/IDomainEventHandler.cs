using CABasicCRUD.Domain.Common;

namespace CABasicCRUD.Application.Common.Interfaces;

public interface IDomainEventHandler<TDomainEvent>
    where TDomainEvent : IDomainEvent
{
    Task Handle(TDomainEvent domainEvent, CancellationToken cancellationToken);
}
