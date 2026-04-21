using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Infrastructure.Persistence.Repositories;

public class UbicacionRepository : Repository<Ubicacion>, IUbicacionRepository
{
    public UbicacionRepository(InventarioDbContext dbContext) : base(dbContext)
    {
    }

    public async Task<Ubicacion?> GetByNameAsync(string name, CancellationToken cancellationToken = default)
    {
        return await DbContext.Ubicaciones
            .FirstOrDefaultAsync(u => u.Nombre.ToUpper() == name.ToUpper(), cancellationToken);
    }

    public async Task<IReadOnlyList<Ubicacion>> GetBySearchTermAsync(string termino, CancellationToken cancellationToken = default)
    {
        var query = DbContext.Ubicaciones.AsNoTracking();
        
        if (!string.IsNullOrWhiteSpace(termino))
        {
            var terminoLower = termino.ToLower();
            query = query.Where(u => u.Nombre.ToLower().Contains(terminoLower));
        }

        return await query.Take(10).ToListAsync(cancellationToken);
    }
}