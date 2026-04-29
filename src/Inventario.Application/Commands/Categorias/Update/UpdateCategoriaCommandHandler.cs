using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
using Inventario.Application.Common.Models;

namespace Inventario.Application.Commands.Categorias.Update
{
    internal sealed class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, Result>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoriaCommandHandler(
            ICategoriaRepository categoriaRepository,
            IUbicacionRepository ubicacionRepository,
            IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(request.Id, cancellationToken);
            if (categoria is null)
            {
                return Result.Failure($"La categoría con ID {request.Id} no existe.");
            }

            var existe = await _categoriaRepository.GetByCodeAndUbicacionIdAsync(request.Codigo, request.UbicacionId, cancellationToken);
            if (existe is not null && existe.Id != request.Id)
            {
                return Result.Failure($"El código {request.Codigo} ya existe en esta ubicación");
            }

            var ubicacion = await _ubicacionRepository.GetByIdAsync(request.UbicacionId, cancellationToken);
            if (ubicacion is null)
            {
                return Result.Failure("Debes asignar una ubicación válida para esta categoría.");
            }

            categoria.Update(
                request.Codigo,
                request.Descripcion,
                request.Valores,
                request.UbicacionId
            );

            _categoriaRepository.Update(categoria);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result.Success();
        }
    }
}
