using Inventario.Application.Queries.Usuarios.GetList;
using MediatR;

namespace Inventario.Application.Queries.Usuarios.Search
{
    public record SearchUsuariosQuery(string Termino) : IRequest<IReadOnlyList<UsuarioDto>>;
}