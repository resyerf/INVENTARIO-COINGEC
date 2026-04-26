using MediatR;

namespace Inventario.Application.Commands.Activos.Update
{
    public record UpdateActivoCommand(
        Guid Id,
        string NombreEquipo,
        string? CodigoEquipo,
        Guid CategoriaId,
        decimal CostoUnitario,
        int Cantidad,
        string? Marca,
        string? Modelo,
        string? Serie,
        string? Estado,
        string Etiquetado,
        Guid? UbicacionId,
        DateTime? FechaAdquisicion) : IRequest<Unit>;
}
