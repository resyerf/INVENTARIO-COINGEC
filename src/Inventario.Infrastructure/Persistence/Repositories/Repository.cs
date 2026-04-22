using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public abstract class Repository<T> : IRepository<T> where T : AggregateRoot
    {
        protected readonly InventarioDbContext DbContext;

        protected Repository(InventarioDbContext dbContext)
        {
            DbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));
        }

        public virtual async Task<T?> GetByIdAsync(Guid id, CancellationToken cancellationToken = default)
        {
            // Usamos EF.Property para acceder al Id definido en la clase base Entity
            return await DbContext.Set<T>()
                .FirstOrDefaultAsync(e => e.Id == id, cancellationToken);
        }

        public virtual async Task<List<T>> GetAllAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().ToListAsync(cancellationToken);
        }

        public virtual async Task<int> CountAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Set<T>().CountAsync(cancellationToken);
        }

        public void Add(T entity) => DbContext.Set<T>().Add(entity);
        public void AddRange(IEnumerable<T> entities) => DbContext.Set<T>().AddRange(entities);

        public void Update(T entity) => DbContext.Set<T>().Update(entity);

        public void Delete(T entity) => DbContext.Set<T>().Remove(entity);
    }
}