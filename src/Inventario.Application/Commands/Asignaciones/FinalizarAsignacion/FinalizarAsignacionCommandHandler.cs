using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Asignaciones.FinalizarAsignacion
{
    internal sealed class FinalizarAsignacionCommandHandler : IRequestHandler<FinalizarAsignacionCommand, Result>
    {
        private readonly IAsignacionRepository _asignacionRepository;
        private readonly IActivoRepository _activoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public FinalizarAsignacionCommandHandler(
            IAsignacionRepository asignacionRepository,
            IActivoRepository activoRepository,
            IUnitOfWork unitOfWork)
        {
            _asignacionRepository = asignacionRepository ?? throw new ArgumentNullException(nameof(asignacionRepository));
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(FinalizarAsignacionCommand request, CancellationToken cancellationToken)
        {
            // 1. Buscar la asignación activa
            var asignacion = await _asignacionRepository.GetByIdAsync(request.AsignacionId, cancellationToken);

            if (asignacion == null)
                return Result.Failure("La asignación no existe.");

            if (asignacion.FechaDevolucion.HasValue)
                return Result.Failure("Esta asignación ya fue finalizada anteriormente.");

            // 2. Usar el método de comportamiento de la entidad para cerrar el historial
            asignacion.FinalizarAsignacion(request.EstadoRecibido, request.Observaciones);

            // 3. Liberar el Activo (Quitarle el usuario actual)
            var activo = await _activoRepository.GetByIdAsync(asignacion.ActivoId, cancellationToken);
            if (activo != null)
            {
                activo.LiberarCustodio(); // Método que agregaremos a Activo.cs
            }

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
