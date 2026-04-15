using MediatR;

namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record GetUsuariosQuery(string? SearchTerm) : IRequest<IReadOnlyList<UsuarioDto>>;
}
