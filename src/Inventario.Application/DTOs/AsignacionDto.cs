namespace Inventario.Application.DTOs
{
    public record AsignacionDto(
        Guid Id,
        string ActivoNombre,
        string? ActivoSerie,
        string Custodio,
        DateTime FechaAsignacion,
        DateTime? FechaDevolucion,
        string EstadoEntrega,
        string? EstadoRecibido,
        string? Observaciones
    );
}
