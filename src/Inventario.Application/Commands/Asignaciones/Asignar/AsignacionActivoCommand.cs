using MediatR;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    public record AsignacionActivoCommand(Guid ActivoId,
        Guid UsuarioId,
        string EstadoEntrega,
        string? Observaciones
    ) : IRequest<Guid>; // Retorna el ID de la Asignación creada
}
