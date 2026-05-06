namespace Inventario.Application.DTOs
{
    public record MovimientoInsumoDto(
        Guid Id,
        Guid InsumoId,
        int Cantidad,
        string Tipo,
        string Motivo,
        DateTime Fecha,
        Guid? ReferenciaId
    );
}
