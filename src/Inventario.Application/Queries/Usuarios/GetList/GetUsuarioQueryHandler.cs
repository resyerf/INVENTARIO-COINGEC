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

            string Format(string? value, string defaultValue) => string.IsNullOrWhiteSpace(value) ? defaultValue : value;

            return usuarios.Select(u => new UsuarioDto(
                u.Id,
                Format(u.DocumentoIdentidad, "SIN DOCUMENTO"),
                u.NombreCompleto,
                Format(u.Email, "SIN EMAIL"),
                Format(u.Area, "SIN AREA"),
                Format(u.Cargo, "SIN CARGO"),
                Format(u.Sede, "SIN SEDE"),
                u.IsActive)).ToList().AsReadOnly();
        }
    }
}
