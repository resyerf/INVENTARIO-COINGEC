using Inventario.Application.Common.Models;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Ubicaciones.Create
{
    internal sealed class CreateUbicacionCommandHandler : IRequestHandler<CreateUbicacionCommand, Result<Guid>>
    {
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUbicacionCommandHandler(IUbicacionRepository ubicacionRepository, IUnitOfWork unitOfWork)
        {
            _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Guid>> Handle(CreateUbicacionCommand request, CancellationToken cancellationToken)
        {
            var ubicacion = await _ubicacionRepository.GetByNameAsync(request.nombre, cancellationToken);
            if (ubicacion is not null)
            {
                return Result<Guid>.Failure("Ubicacion ya existe");
            }

            ubicacion = Ubicacion.Create(request.nombre, request.descripcion);

            _ubicacionRepository.Add(ubicacion);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return Result<Guid>.Success(ubicacion.Id);
        }
    }
}
