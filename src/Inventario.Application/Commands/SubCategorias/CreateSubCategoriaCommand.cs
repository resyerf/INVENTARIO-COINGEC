using MediatR;

namespace Inventario.Application.Commands.SubCategorias
{
    public record class CreateSubCategoriaCommand(string nombre, Guid categoriaId) : IRequest<Guid>;
}
