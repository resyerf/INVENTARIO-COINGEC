namespace Inventario.Domain.Primitives
{
    public abstract class AggregateRoot : Entity
    {
        private readonly List<DomainEvent> _domainEvents = new();
        public IReadOnlyCollection<DomainEvent> GetDomainEvents() => _domainEvents.AsReadOnly();
        protected void Raise(DomainEvent domainEvent) => _domainEvents.Add(domainEvent);
        public void ClearDomainEvents() => _domainEvents.Clear();

        protected AggregateRoot(Guid id) : base(id) { }
        protected AggregateRoot() { }
    }
}
