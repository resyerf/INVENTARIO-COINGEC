using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.Categorias.Search
{
    public record SearchCategoriasQuery(string Termino) : IRequest<IReadOnlyList<CategoriaDto>>;
}
