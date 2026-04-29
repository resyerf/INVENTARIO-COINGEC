using Inventario.Application.DTOs;
using MediatR;

using Inventario.Application.Common.Models;

namespace Inventario.Application.Queries.Categorias.Search
{
    public record SearchCategoriasQuery(string Termino) : IRequest<Result<IReadOnlyList<CategoriaDto>>>;
}
