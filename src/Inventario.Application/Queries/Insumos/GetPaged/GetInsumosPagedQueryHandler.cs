using Inventario.Application.Common.Models;
using Inventario.Application.Common.Pagination;
using Inventario.Application.DTOs;
using Inventario.Application.Queries.Usuarios.GetList;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;

namespace Inventario.Application.Queries.Insumos.GetPaged
{
    //internal sealed class GetUsuarioQueryHandler : IRequestHandler<GetUsuariosQuery, Result<PagedResult<UsuarioDto>>>
    internal sealed class GetInsumosPagedQueryHandler : IRequestHandler<GetInsumosPagedQuery, Result<PagedResult<InsumoDto>>>
    {
        private readonly IInsumoRepository _repository;

        public GetInsumosPagedQueryHandler(IInsumoRepository repository)
        {
            _repository = repository;
        }

        public async Task<Result<PagedResult<InsumoDto>>> Handle(GetInsumosPagedQuery request, CancellationToken cancellationToken)
        {
            var (items, totalCount) = await _repository.GetPagedAsync(request.Page, request.PageSize, request.SearchTerm, cancellationToken);

            var dtos = items.Select(i => new InsumoDto(
                i.Id,
                i.Nombre,
                i.StockActual,
                i.UnidadMedida,
                i.Descripcion,
                i.CategoriaId,
                i.Categoria?.Descripcion ?? "SIN CATEGORIA"
            )).ToList();

            return Result<PagedResult<InsumoDto>>.Success(new PagedResult<InsumoDto>(dtos, totalCount, request.Page, request.PageSize));
        }
    }
}
