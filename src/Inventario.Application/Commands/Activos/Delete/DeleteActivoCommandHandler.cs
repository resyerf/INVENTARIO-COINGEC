using Inventario.Domain.Interfaces.Repositories;
using MediatR;
namespace Inventario.Application.Commands.Activos.Delete
{
    internal sealed class DeleteActivoCommandHandler : IRequestHandler<DeleteActivoCommand>
    {
        private readonly IActivoRepository _repository;
        private readonly Inventario.Domain.Primitives.IUnitOfWork _unitOfWork;
        public DeleteActivoCommandHandler(IActivoRepository repository, Inventario.Domain.Primitives.IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteActivoCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) throw new Exception("Activo no encontrado.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
