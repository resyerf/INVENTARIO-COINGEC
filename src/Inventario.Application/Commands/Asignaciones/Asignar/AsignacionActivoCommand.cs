using MediatR;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    public record AsignacionActivoCommand(Guid ActivoId,
        Guid UsuarioId,
        DateTime FechaAsignacion,
        string? Observaciones
    ) : IRequest<Guid>; // Retorna el ID de la Asignación creada
}
