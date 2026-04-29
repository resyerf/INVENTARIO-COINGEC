using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
namespace Inventario.Application.Commands.Activos.Delete
{
    internal sealed class DeleteActivoCommandHandler : IRequestHandler<DeleteActivoCommand, Result>
    {
        private readonly IActivoRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteActivoCommandHandler(IActivoRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result> Handle(DeleteActivoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("Activo no encontrado.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
