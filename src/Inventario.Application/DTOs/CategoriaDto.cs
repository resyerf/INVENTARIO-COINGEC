namespace Inventario.Application.DTOs
{
    public record CategoriaDto(
        Guid Id,
        string Codigo,
        string Descripcion,
        string Valores,
        string Ubicacion,
        string UbicacionDescripcion,
        bool IsActive);
}
