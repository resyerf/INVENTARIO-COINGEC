using MediatR;

namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record GetUsuariosQuery() : IRequest<IReadOnlyList<UsuarioDto>>;
}
