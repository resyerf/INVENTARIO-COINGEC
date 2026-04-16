namespace Inventario.Application.DTOs
{
    public record ActivoDto(
        Guid Id,
        string NombreEquipo,
        string? Marca,
        string? Modelo,
        string? Serie,
        string Etiquetado,
        int Cantidad,
        string? Estado,
        decimal CostoUnitario,
        string? Observaciones,
        string SubCategoria,
        string? Custodio,
        string? Ubicacion,
        DateTime? FechaAdquisicion);
}
