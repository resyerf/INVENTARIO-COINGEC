namespace Inventario.Application.DTOs
{
    public record ActivoReporteDto(
        string NombreEquipo,
        string Marca,
        string Modelo,
        string Serie,
        string Etiquetado,
        string Estado,
        string SubCategoriaNombre,
        string CategoriaCodigo, // Traído desde la relación del padre
        string UbicacionNombre,
        string Responsable,
        decimal CostoUnitario,
        int Cantidad,
        string FechaAdquisicion
    );
}
