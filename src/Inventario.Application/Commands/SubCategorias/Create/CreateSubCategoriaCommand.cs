using MediatR;

namespace Inventario.Application.Commands.SubCategorias.Create
{
    public record class CreateSubCategoriaCommand(string nombre, Guid categoriaId) : IRequest<Guid>;
}
