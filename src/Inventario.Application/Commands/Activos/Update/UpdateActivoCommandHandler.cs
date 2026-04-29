using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Activos.Update
{
    internal sealed class UpdateActivoCommandHandler : IRequestHandler<UpdateActivoCommand, Result>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateActivoCommandHandler(
            IActivoRepository activoRepository,
            ICategoriaRepository categoriaRepository,
            IUbicacionRepository ubicacionRepository,
            IUnitOfWork unitOfWork)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(UpdateActivoCommand request, CancellationToken cancellationToken)
        {
            var activo = await _activoRepository.GetByIdAsync(request.Id, cancellationToken);
            if (activo is null)
            {
                return Result.Failure($"El activo con ID {request.Id} no existe.");
            }

            var categoria = await _categoriaRepository.GetByIdAsync(request.CategoriaId, cancellationToken);
            if (categoria is null)
            {
                return Result.Failure("La categoría especificada no existe.");
            }

            if (request.UbicacionId.HasValue)
            {
                var ubicacion = await _ubicacionRepository.GetByIdAsync(request.UbicacionId.Value, cancellationToken);
                if (ubicacion is null)
                {
                    return Result.Failure("La ubicación especificada no existe.");
                }
            }

            activo.Update(
                request.NombreEquipo,
                request.CodigoEquipo,
                request.CategoriaId,
                request.CostoUnitario,
                request.Cantidad,
                request.Marca,
                request.Modelo,
                request.Serie,
                request.Estado,
                request.Etiquetado,
                request.UbicacionId,
                request.FechaAdquisicion
            );

            _activoRepository.Update(activo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
