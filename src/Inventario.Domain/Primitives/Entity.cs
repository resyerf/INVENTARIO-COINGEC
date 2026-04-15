namespace Inventario.Domain.Primitives
{
    public abstract class Entity
    {
        public Guid Id { get; protected set; }
        protected Entity(Guid id) => Id = id;
        protected Entity() { } // Requerido por EF Core
    }
}
