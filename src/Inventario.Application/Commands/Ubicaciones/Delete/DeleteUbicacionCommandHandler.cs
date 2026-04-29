using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
namespace Inventario.Application.Commands.Ubicaciones.Delete
{
    internal sealed class DeleteUbicacionCommandHandler : IRequestHandler<DeleteUbicacionCommand, Result>
    {
        private readonly IUbicacionRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUbicacionCommandHandler(IUbicacionRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result> Handle(DeleteUbicacionCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("Ubicación no encontrada.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
