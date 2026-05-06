using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Solicitudes.Crear
{
    public record CrearSolicitudCommand(
        Guid InsumoId,
        int Cantidad,
        Guid UsuarioId,
        string? Observaciones
    ) : IRequest<Guid>;

    public class CrearSolicitudCommandHandler : IRequestHandler<CrearSolicitudCommand, Guid>
    {
        private readonly ISolicitudInsumoRepository _solicitudRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CrearSolicitudCommandHandler(ISolicitudInsumoRepository solicitudRepository, IUnitOfWork unitOfWork)
        {
            _solicitudRepository = solicitudRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CrearSolicitudCommand request, CancellationToken cancellationToken)
        {
            var solicitud = SolicitudInsumo.Create(
                request.InsumoId,
                request.Cantidad,
                request.UsuarioId,
                request.Observaciones
            );

            _solicitudRepository.Add(solicitud);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return solicitud.Id;
        }
    }
}
