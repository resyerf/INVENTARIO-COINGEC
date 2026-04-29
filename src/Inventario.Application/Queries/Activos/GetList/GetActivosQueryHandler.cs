using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.GetList
{
    internal sealed class GetActivosQueryHandler : IRequestHandler<GetActivosQuery, Result<PagedResult<ActivoDto>>>
    {
        private readonly IActivoRepository _activoRepository;

        public GetActivosQueryHandler(IActivoRepository activoRepository)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
        }

        public async Task<Result<PagedResult<ActivoDto>>> Handle(GetActivosQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _activoRepository.GetPagedActivosAsync(
                request.Page,
                request.PageSize,
                request.SearchTerm,
                request.Condicion,
                request.IsActive,
                request.Categoria,
                request.Custodio,
                cancellationToken);
            
            var dtos = items.Select(a => new ActivoDto(
                a.Id,
                a.NombreEquipo,
                a.CodigoEquipo,
                a.Marca,
                a.Modelo,
                a.Serie,
                a.Etiquetado,
                a.Cantidad,
                a.Estado,
                a.CostoUnitario,
                a.Observaciones,
                a.Categoria?.Descripcion ?? "Sin Categoria",
                a.Usuario != null ? a.Usuario.NombreCompleto : "Sin Asignar",
                a.Ubicacion?.Nombre ?? "Sin Ubicacion",
                a.FechaAdquisicion,
                a.IsActive
            )).ToList().AsReadOnly();

            return Result<PagedResult<ActivoDto>>.Success(new PagedResult<ActivoDto>(dtos, totalCount, request.Page, request.PageSize));
        }
    }
}
