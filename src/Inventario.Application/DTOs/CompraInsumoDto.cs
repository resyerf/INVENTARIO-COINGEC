namespace Inventario.Application.DTOs
{
    public record CompraInsumoDto(
        Guid Id,
        Guid InsumoId,
        string InsumoNombre,
        int Cantidad,
        decimal PrecioUnitario,
        DateTime FechaCompra,
        Guid UsuarioId,
        string UsuarioNombre,
        string? Observaciones
    );
}
