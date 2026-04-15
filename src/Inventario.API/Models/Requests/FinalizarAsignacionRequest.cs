namespace Inventario.API.Models.Requests
{
    public record FinalizarAsignacionRequest(string EstadoRecibido, string? Observaciones);
}
