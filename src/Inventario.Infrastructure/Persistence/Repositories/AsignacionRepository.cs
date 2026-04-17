using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class AsignacionRepository : Repository<Asignacion>, IAsignacionRepository
    {
        public AsignacionRepository(InventarioDbContext dbContext) : base(dbContext)
        {

        }

        public async Task<IReadOnlyList<Asignacion>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Asignaciones
                .AsNoTracking()
                .Include(a => a.Activo)
                .Include(a => a.Usuario)
                .ToListAsync(cancellationToken);
        }

        /// <summary>
        /// Obtiene el conteo de asignaciones activas (donde FechaDevolucion es null)
        /// </summary>
        public async Task<int> CountActivosAsignadosAsync(CancellationToken cancellationToken = default)
        {
            return await DbContext.Asignaciones
                .Where(a => a.FechaDevolucion == null)
                .CountAsync(cancellationToken);
        }
    }
}
