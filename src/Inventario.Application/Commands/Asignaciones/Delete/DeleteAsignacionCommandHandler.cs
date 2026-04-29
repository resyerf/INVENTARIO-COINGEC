using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
namespace Inventario.Application.Commands.Asignaciones.Delete
{
    internal sealed class DeleteAsignacionCommandHandler : IRequestHandler<DeleteAsignacionCommand, Result>
    {
        private readonly IAsignacionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteAsignacionCommandHandler(IAsignacionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task<Result> Handle(DeleteAsignacionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("Asignación no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
