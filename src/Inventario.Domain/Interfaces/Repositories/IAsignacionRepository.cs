using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface IAsignacionRepository : IRepository<Asignacion>
    {
        Task<IReadOnlyList<Asignacion>> GetAllWithIncludesAsync(CancellationToken cancellationToken = default);
        
        /// <summary>
        /// Obtiene el conteo de asignaciones activas (sin devolución)
        /// </summary>
        Task<int> CountActivosAsignadosAsync(CancellationToken cancellationToken = default);
    }
}
