using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface IMovimientoInsumoRepository : IRepository<MovimientoInsumo>
    {
        Task<IReadOnlyList<MovimientoInsumo>> GetByInsumoIdAsync(Guid insumoId, CancellationToken cancellationToken);
    }
}
