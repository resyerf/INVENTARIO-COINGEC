using Inventario.Application.Common.Pagination;
using MediatR;

namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record GetUsuariosQuery(
        int Page = 1,
        int PageSize = 10,
        string? SearchTerm = null
    ) : IRequest<PagedResult<UsuarioDto>>;
}
