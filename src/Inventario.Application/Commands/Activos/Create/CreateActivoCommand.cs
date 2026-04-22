using MediatR;

namespace Inventario.Application.Commands.Activos.Create
{
    public record CreateActivoCommand(string NombreEquipo,
        string CodigoEquipo,
        Guid SubCategoriaId,
        decimal CostoUnitario,
        int Cantidad,
        string? Marca,
        string? Modelo,
        string? Serie,
        string Etiquetado,
        Guid? UbicacionId,
        DateTime? FechaAdquisicion,
        Guid? UsuarioId // Opcional, por si se asigna al crear
    ) : IRequest<Guid>; // Retorna el ID del nuevo Activo
}
