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

        public async Task<(IReadOnlyList<Asignacion> Items, int TotalCount)> GetPagedAsignacionesAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = DbContext.Asignaciones
                .AsNoTracking()
                .Include(a => a.Activo)
                .Include(a => a.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToLower();
                query = query.Where(a => 
                    (a.Activo != null && a.Activo.NombreEquipo.ToLower().Contains(search)) ||
                    (a.Activo != null && a.Activo.Serie != null && a.Activo.Serie.ToLower().Contains(search)) ||
                    (a.Usuario != null && a.Usuario.NombreCompleto.ToLower().Contains(search))
                );
            }

            var totalCount = await query.CountAsync(cancellationToken);

            var items = await query
                .OrderByDescending(a => a.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
