using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Activos.Create
{
    internal sealed class CreateActivoCommandHandler : IRequestHandler<CreateActivoCommand, Guid>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateActivoCommandHandler(IActivoRepository activoRepository, IUnitOfWork unitOfWork)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Guid> Handle(CreateActivoCommand request, CancellationToken cancellationToken)
        {
            var activo = Activo.Create(
                request.NombreEquipo,
                request.CodigoEquipo,
                //request.SubCategoriaId,
                request.CategoriaId,
                request.CostoUnitario,
                request.Cantidad,
                request.Marca,
                request.Modelo,
                request.Serie,
                request.EstadoCondicion,
                request.Etiquetado,
                request.UbicacionId,
                request.FechaAdquisicion
            );

            // 2. Persistir en el repositorio
            _activoRepository.Add(activo);

            // 3. Guardar cambios a través de Unit of Work
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return activo.Id;
        }
    }
}
