using System.Collections.Frozen;
using Inventario.Application.DTOs;
using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Activos.Import;

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

        List<string?> codigosCats = items
            .Select(x => x.Tipo?.Trim().ToUpperInvariant())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct().ToList();

        List<string?> nombresUbis = items
            .Select(x => x.Ubicacion?.Trim().ToUpperInvariant())
            .Where(x => !string.IsNullOrEmpty(x))
            .Distinct().ToList();

        IReadOnlyList<SubCategoria> subCategoriasDb = await subCategoryRepository.GetByCategoriaByListCodeAsync(codigosCats!, cancellationToken);
        IReadOnlyList<Ubicacion> ubicacionesDb = await ubicacionRepository.SearchByListCodeAsync(nombresUbis!, cancellationToken);
        
        FrozenDictionary<string, SubCategoria> subCatDict = subCategoriasDb
            .GroupBy(s => s.Categoria.Codigo.ToUpperInvariant())
            .ToFrozenDictionary(
                g => g.Key,
                g => g.First()
            );

        Dictionary<string, Ubicacion> ubicacionDict = ubicacionesDb.ToDictionary(u => u.Nombre.ToUpperInvariant());

        List<Activo> nuevosActivos = new ();
        List<Ubicacion> nuevasUbicaciones = new();

        foreach (ActivoImportDto item in items)
        {
            var tipoKey = item.Tipo?.Trim().ToUpperInvariant();
            if (string.IsNullOrEmpty(tipoKey) || !subCatDict.TryGetValue(tipoKey, out var subCat))
                continue;

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
                item.CodigoEquipo,
                subCat.Id,
                item.CostoUnitario ?? 0,
                item.Cantidad,
                item.Marca,
                item.Modelo,
                item.Serie,
                item.Etiquetado,
                ubicacion?.Id,
                item.FechaAdquisicion
            ));
        }

        if (nuevasUbicaciones.Count > 0) ubicacionRepository.AddRange(nuevasUbicaciones);
        if (nuevosActivos.Count > 0) activoRepository.AddRange(nuevosActivos);

        await unitOfWork.SaveChangesAsync(cancellationToken);

        return Unit.Value;
    }
}