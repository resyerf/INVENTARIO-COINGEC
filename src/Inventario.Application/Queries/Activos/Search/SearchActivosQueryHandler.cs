using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Activos.Search
{
    internal sealed class SearchActivosQueryHandler : IRequestHandler<SearchActivosQuery, Result<IReadOnlyList<ActivoDto>>>
    {
        private readonly IActivoRepository _repository;

        public SearchActivosQueryHandler(IActivoRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<Result<IReadOnlyList<ActivoDto>>> Handle(SearchActivosQuery request, CancellationToken cancellationToken)
        {
            if (string.IsNullOrWhiteSpace(request.Termino)) return Result<IReadOnlyList<ActivoDto>>.Success(new List<ActivoDto>().AsReadOnly());

            // Obtener todos los activos y filtrar por término si se proporciona
            var activos = await _repository.GetBySearchTermAsync(request.Termino ,cancellationToken);

            return Result<IReadOnlyList<ActivoDto>>.Success(activos
                .Select(a => new ActivoDto(
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
                    a.Categoria?.Descripcion ?? "",
                    a.Usuario?.NombreCompleto ?? "",
                    a.Ubicacion?.Nombre ?? "",
                    a.FechaAdquisicion,
                    a.IsActive))
                .ToList()
                .AsReadOnly());
        }
    }
}