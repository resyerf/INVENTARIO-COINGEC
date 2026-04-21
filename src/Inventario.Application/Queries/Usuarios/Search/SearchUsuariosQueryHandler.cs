using Inventario.Domain.Interfaces.Repositories;
using Inventario.Application.Queries.Usuarios.GetList;
using MediatR;

namespace Inventario.Application.Queries.Usuarios.Search
{
    internal sealed class SearchUsuariosQueryHandler : IRequestHandler<SearchUsuariosQuery, IReadOnlyList<UsuarioDto>>
    {
        private readonly IUsuarioRepository _repository;

        public SearchUsuariosQueryHandler(IUsuarioRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<UsuarioDto>> Handle(SearchUsuariosQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Termino)) return new List<UsuarioDto>().AsReadOnly();

            var usuarios = await _repository.GetBySearchTermAsync(request.Termino, cancellationToken);

            return usuarios
                .Select(u => new UsuarioDto(
                    u.Id,
                    u.NombreCompleto,
                    u.Email,
                    u.Area ?? "Sin Area",
                    u.Cargo ?? "Sin Cargo",
                    u.IsActive))
                .ToList()
                .AsReadOnly();
        }
    }
}