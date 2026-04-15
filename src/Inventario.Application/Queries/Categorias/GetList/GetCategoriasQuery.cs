using MediatR;

namespace Inventario.Application.Queries.Categorias.GetList
{
    public record GetCategoriasQuery() : IRequest<IReadOnlyList<CategoriaDto>>;
}
