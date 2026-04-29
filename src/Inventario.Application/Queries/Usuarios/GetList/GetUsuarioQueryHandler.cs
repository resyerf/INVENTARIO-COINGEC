using Inventario.Application.Common.Pagination;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Usuarios.GetList
{
    internal sealed class GetUsuarioQueryHandler : IRequestHandler<GetUsuariosQuery, PagedResult<UsuarioDto>>
    {
        private readonly IUsuarioRepository _usuarioRepository;

        public GetUsuarioQueryHandler(IUsuarioRepository usuarioRepository)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        public async Task<PagedResult<UsuarioDto>> Handle(GetUsuariosQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _usuarioRepository.GetPagedUsuariosAsync(
                request.Page, 
                request.PageSize, 
                request.SearchTerm, 
                cancellationToken);

            string Format(string? value, string defaultValue) => string.IsNullOrWhiteSpace(value) ? defaultValue : value;

            var dtos = items.Select(u => new UsuarioDto(
                u.Id,
                Format(u.DocumentoIdentidad, "SIN DOCUMENTO"),
                u.NombreCompleto,
                Format(u.Email, "SIN EMAIL"),
                Format(u.Area, "SIN AREA"),
                Format(u.Cargo, "SIN CARGO"),
                Format(u.Sede, "SIN SEDE"),
                u.IsActive)).ToList().AsReadOnly();

            return new PagedResult<UsuarioDto>(dtos, totalCount, request.Page, request.PageSize);
        }
    }
}
