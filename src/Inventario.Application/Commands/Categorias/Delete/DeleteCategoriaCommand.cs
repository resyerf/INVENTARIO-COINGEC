using MediatR;
namespace Inventario.Application.Commands.Categorias.Delete
{
    public record DeleteCategoriaCommand(Guid Id) : IRequest;
}
