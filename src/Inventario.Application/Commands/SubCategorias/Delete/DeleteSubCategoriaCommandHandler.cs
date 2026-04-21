using Inventario.Domain.Interfaces.Repositories;
using MediatR;
namespace Inventario.Application.Commands.SubCategorias.Delete
{
    internal sealed class DeleteSubCategoriaCommandHandler : IRequestHandler<DeleteSubCategoriaCommand>
    {
        private readonly ISubCategoryRepository _repository;
        private readonly Inventario.Domain.Primitives.IUnitOfWork _unitOfWork;
        public DeleteSubCategoriaCommandHandler(ISubCategoryRepository repository, Inventario.Domain.Primitives.IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteSubCategoriaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) throw new Exception("SubCategoría no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
