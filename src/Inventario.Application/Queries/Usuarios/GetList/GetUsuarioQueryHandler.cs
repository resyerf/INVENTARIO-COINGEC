using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Usuarios.GetList
{
    internal sealed class GetUsuarioQueryHandler : IRequestHandler<GetUsuariosQuery, IReadOnlyList<UsuarioDto>>
    {
        private readonly IUsuarioRepository _repository;

        public GetUsuarioQueryHandler(IUsuarioRepository repository)
        {
            _repository = repository;
        }   
        public async Task<IReadOnlyList<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
        {
            var usuarios = await _repository.GetAllAsync(cancellationToken);
            return usuarios.Select(u => new UsuarioDto(
                u.Id,
                u.NombreCompleto,
                u.Email,
                u.Area ?? "Sin Area",
                u.Cargo ?? "Sin Cargo")).ToList().AsReadOnly();
        }
    }
}
