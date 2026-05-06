using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Insumos.GetHistory
{
    public record GetInsumoHistoryQuery(Guid InsumoId) : IRequest<IReadOnlyList<MovimientoInsumoDto>>;

    public class GetInsumoHistoryQueryHandler : IRequestHandler<GetInsumoHistoryQuery, IReadOnlyList<MovimientoInsumoDto>>
    {
        private readonly IMovimientoInsumoRepository _repository;

        public GetInsumoHistoryQueryHandler(IMovimientoInsumoRepository repository)
        {
            _repository = repository;
        }

        public async Task<IReadOnlyList<MovimientoInsumoDto>> Handle(GetInsumoHistoryQuery request, CancellationToken cancellationToken)
        {
            var items = await _repository.GetByInsumoIdAsync(request.InsumoId, cancellationToken);
            return items.Select(m => new MovimientoInsumoDto(
                m.Id,
                m.InsumoId,
                m.Cantidad,
                m.Tipo.ToString(),
                m.Motivo,
                m.Fecha,
                m.ReferenciaId
            )).ToList();
        }
    }
}
