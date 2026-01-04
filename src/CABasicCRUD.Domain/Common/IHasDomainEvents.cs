namespace CABasicCRUD.Domain.Common;

// marker interface
public interface IHasDomainEvents
{
    IReadOnlyList<IDomainEvent> DomainEvents { get; }
    void ClearDomainEvents();
}
