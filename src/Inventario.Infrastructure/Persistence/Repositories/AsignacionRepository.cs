using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Infrastructure.Persistence.Context;

namespace Inventario.Infrastructure.Persistence.Repositories
{
    public class AsignacionRepository : Repository<Asignacion>, IAsignacionRepository
    {
        public AsignacionRepository(InventarioDbContext dbContext) : base(dbContext)
        {

        }
    }
}
