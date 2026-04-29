using Inventario.Domain.Entities;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Interfaces.Repositories;

public interface IActivoRepository : IRepository<Activo>
{

    // Búsqueda por número de serie (Para evitar duplicados de hardware real)
    Task<Activo?> GetBySerialNumberAsync(string serialNumber, CancellationToken cancellationToken = default);

    // Obtener activos filtrados por categoría (Útil para reportes de Sótano, Taller, etc.)
    Task<List<Activo>> GetByCategoriaAsync(Guid categoriaId, CancellationToken cancellationToken = default);

    // Obtener todos los activos que un usuario tiene actualmente en su poder
    Task<List<Activo>> GetActivosAsignadosAUsuarioAsync(Guid usuarioId, CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Activo>> GetAllForReportAsync(CancellationToken ct);
    
    // Pagination and Filtering
    Task<(IReadOnlyList<Activo> Items, int TotalCount)> GetPagedActivosAsync(
        int pageNumber, 
        int pageSize, 
        string? searchTerm, 
        string? condicion, 
        bool? isActive, 
        string? categoria, 
        string? custodio, 
        CancellationToken cancellationToken = default);

    Task<IReadOnlyList<Activo>> GetBySearchTermAsync(string termino, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<Activo>> GetExistingCodesAsync(List<string> codigosEquipo, CancellationToken cancellationToken = default);
}