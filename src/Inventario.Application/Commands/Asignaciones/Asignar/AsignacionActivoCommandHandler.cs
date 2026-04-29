using Inventario.Application.Common.Models;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    internal sealed class AsignacionActivoCommandHandler : IRequestHandler<AsignacionActivoCommand, Result<Guid>>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IAsignacionRepository _asignacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public AsignacionActivoCommandHandler(IActivoRepository activoRepository, IAsignacionRepository asignacionRepository, IUnitOfWork unitOfWork)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
            _asignacionRepository = asignacionRepository ?? throw new ArgumentNullException(nameof(asignacionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result<Guid>> Handle(AsignacionActivoCommand request, CancellationToken cancellationToken)
        {
            // 1. Obtener el activo (con sus asignaciones actuales para validar)
            var activo = await _activoRepository.GetByIdAsync(request.ActivoId, cancellationToken);

            if (activo == null)
                return Result<Guid>.Failure("El activo no existe.");

            // Validar que el activo no esté ya asignado
            if (activo.UsuarioId.HasValue)
            {
                return Result<Guid>.Failure($"El activo seleccionado ya se encuentra asignado actualmente.");
            }

            // 2. Lógica de Dominio: Crear la asignación
            // Nota: Idealmente, el Activo debería tener un método .Asignar(usuarioId, estado)
            var asignacion = Asignacion.Create(
                request.ActivoId,
                request.UsuarioId,
                request.FechaAsignacion,
                activo.Estado ?? "Sin informacion",
                request.Observaciones ?? "Sin observaciones"
            );

            // 3. Actualizar el estado del Activo (Quién lo tiene ahora)
            // Agregaremos un método en Activo para esto
            activo.SetCustodio(request.UsuarioId);

            // 4. Persistir
            _asignacionRepository.Add(asignacion);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(asignacion.Id);
        }
    }
}
