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
            var usuarios = await _repository.GetAllAsync(cancellationToken);

            var filtered = usuarios
                .Where(u => string.IsNullOrWhiteSpace(request.Termino) ||
                            u.NombreCompleto.Contains(request.Termino, StringComparison.OrdinalIgnoreCase) ||
                            u.Email.Contains(request.Termino, StringComparison.OrdinalIgnoreCase) ||
                            u.DocumentoIdentidad.Contains(request.Termino, StringComparison.OrdinalIgnoreCase))
                .Select(u => new UsuarioDto(
                    u.Id,
                    u.NombreCompleto,
                    u.Email,
                    u.Area ?? "Sin Area",
                    u.Cargo ?? "Sin Cargo"))
                .ToList()
                .AsReadOnly();

            return filtered;
        }
    }
}