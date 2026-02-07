namespace CABasicCRUD.Domain.Common;

public abstract class AggregateRoot<TId> : EntityBase<TId>, IHasDomainEvents
    where TId : EntityIdBase
{
    private readonly List<IDomainEvent> _domainEvents = new();

    protected AggregateRoot(TId? id)
        : base(id) { }

    public IReadOnlyList<IDomainEvent> DomainEvents => _domainEvents.AsReadOnly();

    protected void AddDomainEvent(IDomainEvent domainEvent)
    {
        _domainEvents.Add(domainEvent);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
