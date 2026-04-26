using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Ubicaciones.Update
{
    internal sealed class UpdateUbicacionCommandHandler : IRequestHandler<UpdateUbicacionCommand, Unit>
    {
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUbicacionCommandHandler(
            IUbicacionRepository ubicacionRepository,
            IUnitOfWork unitOfWork)
        {
            _ubicacionRepository = ubicacionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateUbicacionCommand request, CancellationToken cancellationToken)
        {
            var ubicacion = await _ubicacionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (ubicacion is null)
            {
                throw new Exception($"La ubicación con ID {request.Id} no existe.");
            }

            ubicacion.Update(
                request.Nombre,
                request.Descripcion
            );

            _ubicacionRepository.Update(ubicacion);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
