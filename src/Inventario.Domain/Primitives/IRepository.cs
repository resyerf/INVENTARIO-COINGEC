namespace Inventario.Domain.Primitives
{
    public interface IRepository<T> where T : AggregateRoot
    {
        Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default);
        Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default);
        Task<int> CountAsync(CancellationToken cancellationToken = default);
        void Add(T entity);
        void Update(T entity);
        void Delete(T entity);
    }
}
