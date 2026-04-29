using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
namespace Inventario.Application.Commands.SubCategorias.Delete
{
    internal sealed class DeleteSubCategoriaCommandHandler : IRequestHandler<DeleteSubCategoriaCommand, Result>
    {
        private readonly ISubCategoryRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteSubCategoriaCommandHandler(ISubCategoryRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result> Handle(DeleteSubCategoriaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("SubCategoría no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
