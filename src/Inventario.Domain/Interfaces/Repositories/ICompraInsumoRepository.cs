using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface ICompraInsumoRepository : IRepository<CompraInsumo>
    {
        Task<(IReadOnlyList<CompraInsumo> Items, int TotalCount)> GetPagedAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}
