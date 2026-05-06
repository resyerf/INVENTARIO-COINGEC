using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Solicitudes.Rechazar
{
    public record RechazarSolicitudCommand(
        Guid SolicitudId,
        string? RespuestaAdmin
    ) : IRequest<Unit>;

    public class RechazarSolicitudCommandHandler : IRequestHandler<RechazarSolicitudCommand, Unit>
    {
        private readonly ISolicitudInsumoRepository _solicitudRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RechazarSolicitudCommandHandler(ISolicitudInsumoRepository solicitudRepository, IUnitOfWork unitOfWork)
        {
            _solicitudRepository = solicitudRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(RechazarSolicitudCommand request, CancellationToken cancellationToken)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(request.SolicitudId, cancellationToken);
            if (solicitud == null) throw new Exception("Solicitud no encontrada");

            solicitud.Rechazar(request.RespuestaAdmin);

            _solicitudRepository.Update(solicitud);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
