using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.SubCategorias.GetList
{
    public sealed record GetSubCategoriasQuery() : IRequest<IReadOnlyList<SubCategoriaDto>>;
}
