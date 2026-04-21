using MediatR;
namespace Inventario.Application.Commands.SubCategorias.Delete
{
    public record DeleteSubCategoriaCommand(Guid Id) : IRequest;
}
