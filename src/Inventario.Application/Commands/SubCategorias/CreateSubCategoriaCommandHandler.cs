using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.SubCategorias
{
    internal sealed class CreateSubCategoriaCommandHandler : IRequestHandler<CreateSubCategoriaCommand, Guid>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateSubCategoriaCommandHandler(ICategoriaRepository categoriaRepository, ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateSubCategoriaCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriaRepository.GetByIdAsync(request.categoriaId);

            if (category is null)
            {
                throw new Exception($"El código {request.categoriaId} no existe");
            }
            var categoria = SubCategoria.Create(
                request.nombre,
                request.categoriaId
            );

            _subCategoryRepository.Add(categoria);

            // 3. Persistencia
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return categoria.Id;
        }
    }
}
