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
}