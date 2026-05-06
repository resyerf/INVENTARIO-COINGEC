using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Compras.GetPaged
{
    public record GetComprasPagedQuery(int PageNumber, int PageSize, string? SearchTerm) 
        : IRequest<Result<PagedResult<CompraInsumoDto>>>;

    public class GetComprasPagedQueryHandler : IRequestHandler<GetComprasPagedQuery, Result<PagedResult<CompraInsumoDto>>>
    {
        private readonly ICompraInsumoRepository _repository;

        public GetComprasPagedQueryHandler(ICompraInsumoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<CompraInsumoDto>>> Handle(GetComprasPagedQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(request.PageNumber, request.PageSize, request.SearchTerm, cancellationToken);
            
            var dtos = items.Select(c => new CompraInsumoDto(
                c.Id,
                c.InsumoId,
                c.Insumo?.Nombre ?? "DESCONOCIDO",
                c.Cantidad,
                c.PrecioUnitario,
                c.FechaCompra,
                c.UsuarioId,
                c.Usuario?.NombreCompleto ?? "DESCONOCIDO",
                c.Observaciones
            )).ToList();

            var pagedResult = new PagedResult<CompraInsumoDto>(dtos, totalCount, request.PageNumber, request.PageSize);
            return Result<PagedResult<CompraInsumoDto>>.Success(pagedResult);
        }
    }
}
