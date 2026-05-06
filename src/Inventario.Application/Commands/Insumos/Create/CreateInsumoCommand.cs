using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Insumos.Create
{
    public record CreateInsumoCommand(
        string Nombre,
        string UnidadMedida,
        Guid CategoriaId,
        string? Descripcion
    ) : IRequest<Guid>;

    public class CreateInsumoCommandHandler : IRequestHandler<CreateInsumoCommand, Guid>
    {
        private readonly IInsumoRepository _insumoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateInsumoCommandHandler(IInsumoRepository insumoRepository, IUnitOfWork unitOfWork)
        {
            _insumoRepository = insumoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateInsumoCommand request, CancellationToken cancellationToken)
        {
            var insumo = Insumo.Create(
                request.Nombre,
                request.UnidadMedida,
                request.CategoriaId,
                request.Descripcion
            );

            _insumoRepository.Add(insumo);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return insumo.Id;
        }
    }
}
