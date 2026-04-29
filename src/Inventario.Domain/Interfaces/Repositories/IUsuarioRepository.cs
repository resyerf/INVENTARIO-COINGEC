using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories
{
    public interface IUsuarioRepository : IRepository<Usuario>
    {
        // Buscar por nombre completo para filtros de búsqueda
        Task<Usuario?> GetByFullNameAsync(string nombreCompleto, CancellationToken cancellationToken = default);

        // Listar personal por departamento o área (TI, Contabilidad, Campo)
        Task<List<Usuario>> GetByAreaAsync(string area, CancellationToken cancellationToken = default);
        Task<Usuario?> GetByDocumentNbrAsync(string documentNbr, CancellationToken cancellation = default);
        Task<Usuario?> GetByEmailAsync(string email, CancellationToken cancellation = default);
        Task<IReadOnlyList<Usuario>> GetBySearchTermAsync(string termino, CancellationToken cancellationToken = default);
        Task<IReadOnlyList<Usuario>> GetByDocumentNbrListAsync(List<string> dcuments, CancellationToken cancellationToken = default);
        Task<(IReadOnlyList<Usuario> Items, int TotalCount)> GetPagedUsuariosAsync(int pageNumber, int pageSize, string? searchTerm, CancellationToken cancellationToken);
    }
}
