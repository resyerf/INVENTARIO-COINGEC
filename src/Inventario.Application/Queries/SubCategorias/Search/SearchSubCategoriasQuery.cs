using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.SubCategorias.Search
{
    public record SearchSubCategoriasQuery(string Termino) : IRequest<IReadOnlyList<SubCategoriaDto>>;
}
