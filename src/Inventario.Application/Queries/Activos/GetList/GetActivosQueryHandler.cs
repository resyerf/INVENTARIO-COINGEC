using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.GetList
{
    internal sealed class GetActivosQueryHandler : IRequestHandler<GetActivosQuery, IReadOnlyList<ActivoDto>>
    {
        private readonly IActivoRepository _activoRepository;

        public GetActivosQueryHandler(IActivoRepository activoRepository)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
        }

        public async Task<IReadOnlyList<ActivoDto>> Handle(GetActivosQuery request, CancellationToken cancellationToken)
        {
            var activos = await _activoRepository.GetAllForReportAsync(cancellationToken);
            
            return activos.Select(a => new ActivoDto(
                a.Id,
                a.NombreEquipo,
                a.Marca,
                a.Modelo,
                a.Serie,
                a.Etiquetado,
                a.Cantidad,
                a.Estado,
                a.CostoUnitario,
                a.Observaciones,
                a.SubCategoria?.Nombre ?? "Sin Categoria",
                a.Usuario != null ? a.Usuario.NombreCompleto : "Sin Asignar",
                a.Ubicacion?.Nombre ?? "Sin Ubicacion",
                a.FechaAdquisicion,
                a.IsActive
            )).ToList().AsReadOnly();
        }
    }
}
