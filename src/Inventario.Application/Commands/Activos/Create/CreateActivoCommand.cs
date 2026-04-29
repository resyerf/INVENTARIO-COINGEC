using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Commands.Activos.Create
{
    public record CreateActivoCommand(
        string NombreEquipo,
        string CodigoEquipo,
        //Guid SubCategoriaId,
        Guid CategoriaId,
        decimal CostoUnitario,
        int Cantidad,
        string? Marca,
        string? Modelo,
        string? Serie,
        string? EstadoCondicion,
        string Etiquetado,
        Guid? UbicacionId,
        DateTime? FechaAdquisicion,
        Guid? UsuarioId // Opcional, por si se asigna al crear
    ) : IRequest<Result<Guid>>; // Retorna el ID del nuevo Activo
}
