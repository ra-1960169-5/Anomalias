namespace Anomalias.Shared;
public interface IAggregateRoot
{
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents();
    public void ClearDomainEvents();
    public void AddDomainEvent(IDomainEvent domainEvent);

}
