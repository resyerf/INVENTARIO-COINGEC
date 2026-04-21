namespace Inventario.Application.DTOs
{
    public record SubCategoriaDto(Guid Id, string Nombre, string CategoriaCodigo, string CategoriaDescripcion, bool IsActive);
}
