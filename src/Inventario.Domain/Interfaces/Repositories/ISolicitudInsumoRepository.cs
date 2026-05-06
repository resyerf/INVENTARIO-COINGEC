using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface ISolicitudInsumoRepository : IRepository<SolicitudInsumo>
    {
        Task<(IReadOnlyList<SolicitudInsumo> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
        Task<IReadOnlyList<SolicitudInsumo>> GetByUsuarioIdAsync(Guid usuarioId, CancellationToken cancellationToken);
    }
}
