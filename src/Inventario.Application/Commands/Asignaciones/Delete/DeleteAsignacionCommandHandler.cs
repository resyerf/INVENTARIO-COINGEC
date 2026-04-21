using Inventario.Domain.Interfaces.Repositories;
using MediatR;
namespace Inventario.Application.Commands.Asignaciones.Delete
{
    internal sealed class DeleteAsignacionCommandHandler : IRequestHandler<DeleteAsignacionCommand>
    {
        private readonly IAsignacionRepository _repository;
        private readonly Inventario.Domain.Primitives.IUnitOfWork _unitOfWork;
        public DeleteAsignacionCommandHandler(IAsignacionRepository repository, Inventario.Domain.Primitives.IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteAsignacionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) throw new Exception("Asignación no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
