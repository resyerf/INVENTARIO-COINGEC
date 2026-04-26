using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.Reportes
{
    internal sealed class GetActivosReporteQueryHandler : IRequestHandler<GetActivosReporteQuery, IReadOnlyList<ActivoReporteDto>>
    {
        private readonly IActivoRepository _repository;

        public GetActivosReporteQueryHandler(IActivoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }
        public async Task<IReadOnlyList<ActivoReporteDto>> Handle(GetActivosReporteQuery request, CancellationToken cancellationToken)
        {
            // Consultamos al repositorio (asegúrate que el repo incluya las relaciones)
            var activos = await _repository.GetAllForReportAsync(cancellationToken);

            return activos.Select(a => new ActivoReporteDto(
                a.NombreEquipo,
                a.CodigoEquipo ?? string.Empty,
                a.Marca ?? "N/A",
                a.Modelo ?? "N/A",
                a.Serie ?? "N/A",
                a.Etiquetado,
                a.Estado ?? "Nuevo",
                a.Categoria.Descripcion,
                a.Categoria.Codigo, // Acceso multinivel
                a.Ubicacion?.Nombre ?? "Sin Ubicación",
                a.Usuario?.NombreCompleto ?? "No Asignado",
                a.CostoUnitario,
                a.Cantidad,
                a.FechaAdquisicion?.ToString("dd/MM/yyyy") ?? "S/N"
            )).ToList().AsReadOnly();
        }
    }
}
