using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    public record AsignacionActivoCommand(Guid ActivoId,
        Guid UsuarioId,
        DateTime FechaAsignacion,
        string? Observaciones
    ) : IRequest<Result<Guid>>; // Retorna el ID de la Asignación creada
}
