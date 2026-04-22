using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.SubCategorias.CreateMasiv
{
    internal sealed class CreateMasivSubCategoriaCommandHandler : IRequestHandler<CreateMasivSubCategoriaCommand, Unit>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateMasivSubCategoriaCommandHandler(ICategoriaRepository categoriaRepository, ISubCategoryRepository subCategoryRepository, IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Unit> Handle(CreateMasivSubCategoriaCommand request, CancellationToken cancellationToken)
        {
            var category = await _categoriaRepository.GetByIdAsync(request.categoriaId);

            if (category is null)
            {
                throw new Exception($"El código {request.categoriaId} no existe");
            }

            var nombres = request.nombres
                .Split(',')
                .Select(n => n.Trim())
                .Where(n => !string.IsNullOrWhiteSpace(n))
                .Distinct()
                .ToList();

            var subcategorias = new List<SubCategoria>();

            foreach (var nombre in nombres)
            {
                var subcategoria = SubCategoria.Create(nombre, request.categoriaId);
                subcategorias.Add(subcategoria);
            }

            _subCategoryRepository.AddRange(subcategorias);

            await _unitOfWork.SaveChangesAsync(cancellationToken);


            return Unit.Value;
        }
    }
}
