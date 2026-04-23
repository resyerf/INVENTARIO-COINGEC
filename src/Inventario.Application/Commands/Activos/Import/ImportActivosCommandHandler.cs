using System.Collections.Frozen;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Activos.Import
{
    internal sealed class ImportActivosCommandHandler(
    IExcelImportService excelService,
    ISubCategoryRepository subCategoryRepository,
    IUbicacionRepository ubicacionRepository,
    IActivoRepository activoRepository,
    IUnitOfWork unitOfWork) : IRequestHandler<ImportActivosCommand, Unit>
    {
        public async Task<Unit> Handle(ImportActivosCommand request, CancellationToken cancellationToken)
        {
            List<ActivoImportDto> items = excelService.Import<ActivoImportDto>(request.FileStream).ToList();
            if (items.Count == 0) return Unit.Value;

            List<(ActivoImportDto item, string motivo)> errores = new();

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

            IReadOnlyList<SubCategoria> subCategoriasDb = await subCategoryRepository.GetByCategoriaByListCodeAsync(codigosCats!, cancellationToken);
            IReadOnlyList<Ubicacion> ubicacionesDb = await ubicacionRepository.SearchByListCodeAsync(nombresUbis!, cancellationToken);
            IReadOnlyList<Activo> activosConCodigo = await activoRepository.GetExistingCodesAsync(codigosEquiposExcel!, cancellationToken);

            FrozenDictionary<string, SubCategoria> subCatDict = subCategoriasDb
                .GroupBy(s => s.Categoria.Codigo.ToUpperInvariant())
                .ToFrozenDictionary(g => g.Key, g => g.First());

            Dictionary<string, Ubicacion> ubicacionDict =
                ubicacionesDb.ToDictionary(u => u.Nombre.ToUpperInvariant());

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

            foreach (ActivoImportDto item in items)
            {
                string? codigoLimpio = item.CodigoEquipo?.Trim().ToUpperInvariant();

                if (!string.IsNullOrWhiteSpace(codigoLimpio))
                {
                    if (duplicadosExcel.Contains(codigoLimpio))
                    {
                        errores.Add((item, $"El código {codigoLimpio} está duplicado en el archivo"));
                        continue;
                    }

                    if (codigosExistentesSet.Contains(codigoLimpio))
                    {
                        errores.Add((item, $"El código {codigoLimpio} ya está registrado"));
                        continue;
                    }

                    codigosExistentesSet.Add(codigoLimpio);
                }

                var tipoKey = item.Tipo?.Trim().ToUpperInvariant();
                if (string.IsNullOrEmpty(tipoKey) || !subCatDict.TryGetValue(tipoKey, out var subCat))
                {
                    errores.Add((item, "Tipo inválido o no existe"));
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

                if (string.IsNullOrWhiteSpace(item.NombreEquipo))
                {
                    errores.Add((item, "NombreEquipo es obligatorio"));
                    continue;
                }

                nuevosActivos.Add(Activo.Create(
                    item.NombreEquipo,
                    codigoLimpio,
                    subCat.Id,
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

            return Unit.Value;
        }
    }
}