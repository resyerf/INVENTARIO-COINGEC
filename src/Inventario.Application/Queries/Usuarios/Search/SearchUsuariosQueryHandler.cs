using Inventario.Domain.Interfaces.Repositories;
using Inventario.Application.Queries.Usuarios.GetList;
using MediatR;
using Inventario.Application.Common.Models;

namespace Inventario.Application.Queries.Usuarios.Search
{
    internal sealed class SearchUsuariosQueryHandler : IRequestHandler<SearchUsuariosQuery, Result<IReadOnlyList<UsuarioDto>>>
    {
        private readonly IUsuarioRepository _repository;

        public SearchUsuariosQueryHandler(IUsuarioRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<IReadOnlyList<UsuarioDto>>> Handle(SearchUsuariosQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Termino)) return Result<IReadOnlyList<UsuarioDto>>.Success(new List<UsuarioDto>().AsReadOnly());

            var usuarios = await _repository.GetBySearchTermAsync(request.Termino, cancellationToken);
            string Format(string? value, string defaultValue) => string.IsNullOrWhiteSpace(value) ? defaultValue : value;
            return Inventario.Application.Common.Models.Result<IReadOnlyList<UsuarioDto>>.Success(usuarios
                .Select(u => new UsuarioDto(
                    u.Id,
                    Format(u.DocumentoIdentidad, "SIN DOCUMENTO"),
                    u.NombreCompleto,
                    Format(u.Email, "SIN EMAIL"),
                    Format(u.Area, "SIN AREA"),
                    Format(u.Cargo, "SIN CARGO"),
                    Format(u.Sede, "SIN SEDE"),
                    u.IsActive))
                .ToList()
                .AsReadOnly());
        }
    }
}