using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Categorias.Update
{
    internal sealed class UpdateCategoriaCommandHandler : IRequestHandler<UpdateCategoriaCommand, Unit>
    {
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateCategoriaCommandHandler(
            ICategoriaRepository categoriaRepository,
            IUbicacionRepository ubicacionRepository,
            IUnitOfWork unitOfWork)
        {
            _categoriaRepository = categoriaRepository;
            _ubicacionRepository = ubicacionRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Unit> Handle(UpdateCategoriaCommand request, CancellationToken cancellationToken)
        {
            var categoria = await _categoriaRepository.GetByIdAsync(request.Id, cancellationToken);
            if (categoria is null)
            {
                throw new Exception($"La categoría con ID {request.Id} no existe.");
            }

            var existe = await _categoriaRepository.GetByCodeAndUbicacionIdAsync(request.Codigo, request.UbicacionId, cancellationToken);
            if (existe is not null && existe.Id != request.Id)
            {
                throw new Exception($"El código {request.Codigo} ya existe en esta ubicación");
            }

            var ubicacion = await _ubicacionRepository.GetByIdAsync(request.UbicacionId, cancellationToken);
            if (ubicacion is null)
            {
                throw new Exception("Debes asignar una ubicación válida para esta categoría.");
            }

            categoria.Update(
                request.Codigo,
                request.Descripcion,
                request.Valores,
                request.UbicacionId
            );

            _categoriaRepository.Update(categoria);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Unit.Value;
        }
    }
}
