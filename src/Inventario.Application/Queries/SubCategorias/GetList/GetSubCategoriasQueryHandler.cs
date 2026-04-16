using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.SubCategorias.GetList
{
    internal sealed class GetSubCategoriasQueryHandler : IRequestHandler<GetSubCategoriasQuery, IReadOnlyList<SubCategoriaDto>>
    {
        private readonly ISubCategoryRepository _subCategoryRepository;

        public GetSubCategoriasQueryHandler(ISubCategoryRepository subCategoryRepository)
        {
            _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
        }

        public async Task<IReadOnlyList<SubCategoriaDto>> Handle(GetSubCategoriasQuery request, CancellationToken cancellationToken)
        {
            var subCategorias = await _subCategoryRepository.GetAllWithIncludesAsync(cancellationToken);
            
            return subCategorias.Select(s => new SubCategoriaDto(
                s.Id,
                s.Nombre,
                s.Categoria?.Codigo ?? "N/A",
                s.Categoria?.Descripcion ?? "Sin Descripcion"
            )).ToList().AsReadOnly();
        }
    }
}
