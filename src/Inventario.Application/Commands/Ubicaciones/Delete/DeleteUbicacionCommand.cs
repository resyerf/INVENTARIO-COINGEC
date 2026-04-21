using MediatR;
namespace Inventario.Application.Commands.Ubicaciones.Delete
{
    public record DeleteUbicacionCommand(Guid Id) : IRequest;
}
