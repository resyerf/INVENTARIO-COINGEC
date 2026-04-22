using MediatR;

namespace Inventario.Application.Commands.SubCategorias.CreateMasiv
{
    public record class CreateMasivSubCategoriaCommand(string nombres, Guid categoriaId) : IRequest<Unit>;
}
