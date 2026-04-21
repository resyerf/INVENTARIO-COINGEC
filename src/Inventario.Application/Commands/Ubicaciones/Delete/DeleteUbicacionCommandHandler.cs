using Inventario.Domain.Interfaces.Repositories;
using MediatR;
namespace Inventario.Application.Commands.Ubicaciones.Delete
{
    internal sealed class DeleteUbicacionCommandHandler : IRequestHandler<DeleteUbicacionCommand>
    {
        private readonly IUbicacionRepository _repository;
        private readonly Inventario.Domain.Primitives.IUnitOfWork _unitOfWork;
        public DeleteUbicacionCommandHandler(IUbicacionRepository repository, Inventario.Domain.Primitives.IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteUbicacionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) throw new Exception("Ubicación no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
