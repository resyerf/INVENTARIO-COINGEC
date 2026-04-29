using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using MediatR;

namespace Inventario.Application.Queries.SubCategorias.Search
{
    public record SearchSubCategoriasQuery(string Termino) : IRequest<Result<IReadOnlyList<SubCategoriaDto>>>;
}
