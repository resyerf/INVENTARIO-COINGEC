namespace Inventario.Application.DTOs
{
    public record InsumoDto(
        Guid Id,
        string Nombre,
        int StockActual,
        string UnidadMedida,
        string? Descripcion,
        Guid CategoriaId,
        string CategoriaNombre
    );
}
