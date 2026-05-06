namespace Inventario.Application.DTOs
{
    public record SolicitudInsumoDto(
        Guid Id,
        Guid InsumoId,
        string InsumoNombre,
        int Cantidad,
        Guid UsuarioId,
        string UsuarioNombre,
        DateTime FechaSolicitud,
        string Estado,
        string? Observaciones,
        string? RespuestaAdmin,
        DateTime? FechaRespuesta
    );
}
