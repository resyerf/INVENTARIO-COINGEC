using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class SolicitudInsumoRepository : Repository<SolicitudInsumo>, ISolicitudInsumoRepository
    {
        public SolicitudInsumoRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IReadOnlyList<SolicitudInsumo> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = DbContext.SolicitudesInsumos
                .AsNoTracking()
                .Include(s => s.Insumo)
                .Include(s => s.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToUpper();
                query = query.Where(s => s.Insumo.Nombre.Contains(search) || s.Usuario.NombreCompleto.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(s => s.FechaSolicitud)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }

        public async Task<IReadOnlyList<SolicitudInsumo>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken)
        {
            return await DbContext.SolicitudesInsumos
                .AsNoTracking()
                .Include(s => s.Insumo)
                .Where(s => s.UsuarioId == usuarioId)
                .OrderByDescending(s => s.FechaSolicitud)
                .ToListAsync(cancellationToken);
        }
    }
}
