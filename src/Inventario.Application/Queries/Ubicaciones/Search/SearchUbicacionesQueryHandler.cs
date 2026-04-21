using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Ubicaciones.Search
{
    internal sealed class SearchUbicacionesQueryHandler : IRequestHandler<SearchUbicacionesQuery, IReadOnlyList<UbicacionDto>>
    {
        private readonly IUbicacionRepository _repository;

        public SearchUbicacionesQueryHandler(IUbicacionRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        public async Task<IReadOnlyList<UbicacionDto>> Handle(SearchUbicacionesQuery request, CancellationToken cancellationToken)
        {
            // Obtener todas las ubicaciones y filtrar por término si se proporciona
            var ubicaciones = await _repository.GetAllAsync(cancellationToken);

            var filtered = ubicaciones
                .Where(u => string.IsNullOrWhiteSpace(request.Termino) || 
                           u.Nombre.Contains(request.Termino, StringComparison.OrdinalIgnoreCase))
                .Select(u => new UbicacionDto(u.Id, u.Nombre, u.IsActive))
                .ToList()
                .AsReadOnly();

            return filtered;
        }
    }
}
