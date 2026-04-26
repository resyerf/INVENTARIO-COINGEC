using Inventario.Application.DTOs;
using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
using System.Collections.Frozen;

namespace Inventario.Application.Commands.Activos.Import
{
    internal sealed class ImportActivosCommandHandler(
    IExcelImportService excelService,
    ICategoriaRepository categoriaRepository,
    IUbicacionRepository ubicacionRepository,
    IActivoRepository activoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ImportActivosCommand, ImportResult>
    {
        public async Task<ImportResult> Handle(ImportActivosCommand request, CancellationToken cancellationToken)
        {
            List<ActivoImportDto> items = excelService.Import<ActivoImportDto>(request.FileStream).ToList();
            if (items.Count == 0) return new ImportResult(true, 0, 0);

            List<(int RowIndex, string Motivo)> erroresReporte = new();

            List<string?> codigosCats = items
                .Select(x => x.Tipo?.Trim().ToUpperInvariant())
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();

            List<string?> nombresUbis = items
                .Select(x => x.Ubicacion?.Trim().ToUpperInvariant())
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();

            List<string?> codigosEquiposExcel = items
                .Select(x => x.CodigoEquipo?.Trim().ToUpperInvariant())
                .Where(x => !string.IsNullOrEmpty(x))
                .Distinct()
                .ToList();

            IReadOnlyList<Categoria> categoriasDb = await categoriaRepository.GetByListCodeAsync(codigosCats!, cancellationToken);
            IReadOnlyList<Ubicacion> ubicacionesDb = await ubicacionRepository.SearchByListCodeAsync(nombresUbis!, cancellationToken);
            IReadOnlyList<Activo> activosConCodigo = await activoRepository.GetExistingCodesAsync(codigosEquiposExcel!, cancellationToken);

            FrozenDictionary<string, Categoria> catDict = categoriasDb
                .GroupBy(s => s.Codigo.ToUpperInvariant())
                .ToFrozenDictionary(g => g.Key, g => g.First());

            Dictionary<string, Ubicacion> ubicacionDict = ubicacionesDb.ToDictionary(u => u.Nombre.ToUpperInvariant());

            HashSet<string> codigosExistentesSet = activosConCodigo
                .Select(x => x.CodigoEquipo!)
                .ToHashSet(StringComparer.OrdinalIgnoreCase);

            var duplicadosExcel = items
                .Where(x => !string.IsNullOrWhiteSpace(x.CodigoEquipo))
                .GroupBy(x => x.CodigoEquipo!.Trim().ToUpperInvariant())
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToHashSet();

            List<Activo> nuevosActivos = new();
            List<Ubicacion> nuevasUbicaciones = new();

            for (int i = 0; i < items.Count; i++)
            {
                var item = items[i];
                int excelRow = i + 2;
                string? codigoLimpio = item.CodigoEquipo?.Trim().ToUpperInvariant();

                if (!string.IsNullOrWhiteSpace(codigoLimpio))
                {
                    if (duplicadosExcel.Contains(codigoLimpio))
                    {
                        erroresReporte.Add((excelRow, $"El código {codigoLimpio} está duplicado en el archivo"));
                        continue;
                    }

                    if (codigosExistentesSet.Contains(codigoLimpio))
                    {
                        erroresReporte.Add((excelRow, $"El código {codigoLimpio} ya está registrado"));
                        continue;
                    }
                    codigosExistentesSet.Add(codigoLimpio);
                }

                var tipoKey = item.Tipo?.Trim().ToUpperInvariant();
                if (string.IsNullOrEmpty(tipoKey) || !catDict.TryGetValue(tipoKey, out var cat))
                {
                    erroresReporte.Add((excelRow, "Tipo inválido o no existe"));
                    continue;
                }

                if (string.IsNullOrWhiteSpace(item.NombreEquipo))
                {
                    erroresReporte.Add((excelRow, "NombreEquipo es obligatorio"));
                    continue;
                }

                string? ubiKey = item.Ubicacion?.Trim().ToUpperInvariant();
                Ubicacion? ubicacion = null;

                if (!string.IsNullOrEmpty(ubiKey))
                {
                    if (!ubicacionDict.TryGetValue(ubiKey, out ubicacion))
                    {
                        ubicacion = Ubicacion.Create(ubiKey);
                        ubicacionDict.Add(ubiKey, ubicacion);
                        nuevasUbicaciones.Add(ubicacion);
                    }
                }

                nuevosActivos.Add(Activo.Create(
                    item.NombreEquipo,
                    codigoLimpio,
                    cat.Id,
                    item.CostoUnitario ?? 0,
                    item.Cantidad,
                    item.Marca,
                    item.Modelo,
                    item.Serie,
                    item.Estado,
                    item.Etiquetado,
                    ubicacion?.Id,
                    item.FechaAdquisicion
                ));
            }

            if (nuevasUbicaciones.Count > 0)
                ubicacionRepository.AddRange(nuevasUbicaciones);

            if (nuevosActivos.Count > 0)
                activoRepository.AddRange(nuevosActivos);

            await unitOfWork.SaveChangesAsync(cancellationToken);

            if (erroresReporte.Count > 0)
            {
                byte[] fileError = excelService.GenerateErrorReport<ActivoImportDto>(request.FileStream, erroresReporte);
                return new ImportResult(false, 0, erroresReporte.Count, fileError);
            }            

            return new ImportResult(true, nuevosActivos.Count, 0);
        }
    }
}