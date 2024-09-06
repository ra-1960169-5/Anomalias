namespace Anomalias.Shared;
public abstract class AggregateRoot<TId> : Entity<TId> where TId : IEquatable<TId>
{
    protected AggregateRoot(TId id) : base(id) { }

    private readonly List<IDomainEvent> _domainEvents = [];
    public IReadOnlyCollection<IDomainEvent> GetDomainEvents() => _domainEvents.ToList();
    public void ClearDomainEvents() => _domainEvents.Clear();
    public void AddDomainEvent(IDomainEvent domainEvent) => _domainEvents.Add(domainEvent);
}
