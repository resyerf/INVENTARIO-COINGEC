using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.Export
{
    internal sealed class ExportActivosQueryHandler : IRequestHandler<ExportActivosQuery, Result<byte[]>>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IExcelExportService _excelExportService;

        public ExportActivosQueryHandler(IActivoRepository activoRepository, IExcelExportService excelExportService)
        {
            _activoRepository = activoRepository;
            _excelExportService = excelExportService;
        }

        public async Task<Result<byte[]>> Handle(ExportActivosQuery request, CancellationToken cancellationToken)
        {
            var activos = await _activoRepository.GetAllForReportAsync(cancellationToken);

            var reportData = activos.Select(a => new ActivoReporteDto(
                a.NombreEquipo,
                a.CodigoEquipo ?? string.Empty,
                a.Marca ?? string.Empty,
                a.Modelo ?? string.Empty,
                a.Serie ?? string.Empty,
                a.Etiquetado,
                a.Estado ?? string.Empty,
                a.Categoria?.Descripcion?? "Sin Categoría",
                a.Categoria?.Codigo ?? "Sin Codigo de categoría",
                a.Ubicacion?.Nombre ?? "Sin Ubicacion",
                a.Usuario != null ? a.Usuario.NombreCompleto : "Sin Asignar",
                a.CostoUnitario,
                a.Cantidad,
                a.FechaAdquisicion?.ToString("yyyy-MM-dd") ?? string.Empty
            )).ToList();

            var columns = new Dictionary<string, Func<ActivoReporteDto, object?>>
            {
                { "Equipo", x => x.NombreEquipo },
                { "Código", x => x.CodigoEquipo  },
                { "Marca", x => x.Marca },
                { "Modelo", x => x.Modelo },
                { "Serie", x => x.Serie },
                { "Etiquetado", x => x.Etiquetado },
                { "Estado", x => x.Estado },
                { "Categoría", x => x.CategoriaNombre },
                { "Clasificación por Tipo", x => x.CategoriaCodigo },
                { "Ubicación", x => x.UbicacionNombre },
                { "Responsable", x => x.Responsable },
                { "Costo Unitario", x => x.CostoUnitario },
                { "Cantidad", x => x.Cantidad },
                { "Fecha Adquisición", x => x.FechaAdquisicion }
            };

            return Result<byte[]>.Success(_excelExportService.Export(reportData, "Activos", columns));
        }
    }
}
