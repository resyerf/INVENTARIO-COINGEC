using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    internal sealed class AsignacionActivoCommandHandler : IRequestHandler<AsignacionActivoCommand, Guid>
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
        public async Task<Guid> Handle(AsignacionActivoCommand request, CancellationToken cancellationToken)
        {
            // 1. Obtener el activo (con sus asignaciones actuales para validar)
            var activo = await _activoRepository.GetByIdAsync(request.ActivoId, cancellationToken);

            if (activo == null)
                throw new Exception("El activo no existe.");

            // 2. Lógica de Dominio: Crear la asignación
            // Nota: Idealmente, el Activo debería tener un método .Asignar(usuarioId, estado)
            var asignacion = Asignacion.Create(
                activo.Id,
                request.UsuarioId,
                request.EstadoEntrega
            );

            // 3. Actualizar el estado del Activo (Quién lo tiene ahora)
            // Agregaremos un método en Activo para esto
            activo.SetCustodio(request.UsuarioId);

            // 4. Persistir
            _asignacionRepository.Add(asignacion);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return asignacion.Id;
        }
    }
}
