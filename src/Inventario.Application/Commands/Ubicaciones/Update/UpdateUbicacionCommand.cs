using MediatR;

namespace Inventario.Application.Commands.Ubicaciones.Update
{
    public record UpdateUbicacionCommand(
        Guid Id,
        string Nombre,
        string? Descripcion) : IRequest<Unit>;
}
