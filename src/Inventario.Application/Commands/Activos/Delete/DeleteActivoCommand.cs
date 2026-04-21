using MediatR;
namespace Inventario.Application.Commands.Activos.Delete
{
    public record DeleteActivoCommand(Guid Id) : IRequest;
}
