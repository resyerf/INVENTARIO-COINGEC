using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class CompraInsumoRepository : Repository<CompraInsumo>, ICompraInsumoRepository
    {
        public CompraInsumoRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<(IReadOnlyList<CompraInsumo> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken)
        {
            var query = DbContext.ComprasInsumos
                .AsNoTracking()
                .Include(c => c.Insumo)
                .Include(c => c.Usuario)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                var search = searchTerm.ToUpper();
                query = query.Where(c => c.Insumo.Nombre.Contains(search) || c.Observaciones!.Contains(search));
            }

            var totalCount = await query.CountAsync(cancellationToken);
            var items = await query
                .OrderByDescending(c => c.FechaCompra)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync(cancellationToken);

            return (items, totalCount);
        }
    }
}
