using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Ubicaciones.Update
{
    internal sealed class UpdateUbicacionCommandHandler : IRequestHandler<UpdateUbicacionCommand, Result>
    {
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUbicacionCommandHandler(
            IUbicacionRepository ubicacionRepository,
            IUnitOfWork unitOfWork)
        {
            _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(UpdateUbicacionCommand request, CancellationToken cancellationToken)
        {
            var ubicacion = await _ubicacionRepository.GetByIdAsync(request.Id, cancellationToken);
            if (ubicacion is null)
            {
                return Result.Failure($"La ubicación con ID {request.Id} no existe.");
            }

            ubicacion.Update(
                request.Nombre,
                request.Descripcion
            );

            _ubicacionRepository.Update(ubicacion);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
