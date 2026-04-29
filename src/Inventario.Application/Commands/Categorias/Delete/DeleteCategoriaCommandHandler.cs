using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
using Inventario.Application.Common.Models;

namespace Inventario.Application.Commands.Categorias.Delete
{
    internal sealed class DeleteCategoriaCommandHandler : IRequestHandler<DeleteCategoriaCommand, Result>
    {
        private readonly ICategoriaRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteCategoriaCommandHandler(ICategoriaRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result> Handle(DeleteCategoriaCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("Categoría no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
