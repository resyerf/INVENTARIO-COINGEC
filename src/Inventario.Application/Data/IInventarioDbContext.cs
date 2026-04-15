using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Inventario.Application.Data
{
    public interface IInventarioDbContext
    {
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
        DbSet<Activo> Activos { get; }
        DbSet<Categoria> Categorias { get; }
        DbSet<Usuario> Usuarios { get; }
        DbSet<Asignacion> Asignaciones { get; }
        DbSet<Mantenimiento> Mantenimientos { get; }
        DbSet<Ubicacion> Ubicaciones { get; }
        DbSet<SubCategoria> SubCategorias { get; }
    }
}
