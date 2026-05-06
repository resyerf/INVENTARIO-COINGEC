using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class MovimientoInsumoRepository : Repository<MovimientoInsumo>, IMovimientoInsumoRepository
    {
        public MovimientoInsumoRepository(InventarioDbContext dbContext) : base(dbContext)
        {
        }

        public async Task<IReadOnlyList<MovimientoInsumo>> GetByInsumoIdAsync(Guid insumoId, CancellationToken cancellationToken)
        {
            return await DbContext.MovimientosInsumos
                .AsNoTracking()
                .Where(m => m.InsumoId == insumoId)
                .OrderByDescending(m => m.Fecha)
                .ToListAsync(cancellationToken);
        }
    }
}
