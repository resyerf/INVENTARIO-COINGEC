using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.Search
{
    internal sealed class SearchActivosQueryHandler : IRequestHandler<SearchActivosQuery, IReadOnlyList<ActivoDto>>
    {
        private readonly IActivoRepository _repository;

        public SearchActivosQueryHandler(IActivoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<ActivoDto>> Handle(SearchActivosQuery request, CancellationToken cancellationToken)
        {
            // Obtener todos los activos y filtrar por término si se proporciona
            var activos = await _repository.GetAllAsync(cancellationToken);

            var filtered = activos
                .Where(a => string.IsNullOrWhiteSpace(request.Termino) || 
                           a.NombreEquipo.Contains(request.Termino, StringComparison.OrdinalIgnoreCase) ||
                           (a.Serie != null && a.Serie.Contains(request.Termino, StringComparison.OrdinalIgnoreCase)))
                .Select(a => new ActivoDto(
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
                    a.SubCategoria?.Nombre ?? "",
                    a.Usuario?.NombreCompleto ?? "",
                    a.Ubicacion?.Nombre ?? "",
                    a.FechaAdquisicion))
                .ToList()
                .AsReadOnly();

            return filtered;
        }
    }
}