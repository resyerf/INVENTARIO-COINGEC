using Inventario.Domain.Entities;
using Inventario.Domain.Enums;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Solicitudes.Aprobar
{
    public record AprobarSolicitudCommand(
        Guid SolicitudId,
        string? RespuestaAdmin
    ) : IRequest<Unit>;

    public class AprobarSolicitudCommandHandler : IRequestHandler<AprobarSolicitudCommand, Unit>
    {
        private readonly ISolicitudInsumoRepository _solicitudRepository;
        private readonly IInsumoRepository _insumoRepository;
        private readonly IMovimientoInsumoRepository _movimientoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AprobarSolicitudCommandHandler(
            ISolicitudInsumoRepository solicitudRepository,
            IInsumoRepository insumoRepository,
            IMovimientoInsumoRepository movimientoRepository,
            IUnitOfWork unitOfWork)
        {
            _solicitudRepository = solicitudRepository;
            _insumoRepository = insumoRepository;
            _movimientoRepository = movimientoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(AprobarSolicitudCommand request, CancellationToken cancellationToken)
        {
            var solicitud = await _solicitudRepository.GetByIdAsync(request.SolicitudId, cancellationToken);
            if (solicitud == null) throw new Exception("Solicitud no encontrada");

            var insumo = await _insumoRepository.GetByIdAsync(solicitud.InsumoId, cancellationToken);
            if (insumo == null) throw new Exception("Insumo no encontrado");

            // 1. Validar stock
            if (insumo.StockActual < solicitud.Cantidad)
            {
                throw new InvalidOperationException("Stock insuficiente para aprobar esta solicitud.");
            }

            // 2. Aprobar solicitud
            solicitud.Aprobar(request.RespuestaAdmin);

            // 3. Decrementar stock
            insumo.DecrementarStock(solicitud.Cantidad);

            // 4. Registrar movimiento
            var movimiento = MovimientoInsumo.Create(
                solicitud.InsumoId,
                solicitud.Cantidad,
                TipoMovimiento.SALIDA,
                "SOLICITUD APROBADA",
                solicitud.Id
            );

            _solicitudRepository.Update(solicitud);
            _insumoRepository.Update(insumo);
            _movimientoRepository.Add(movimiento);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
