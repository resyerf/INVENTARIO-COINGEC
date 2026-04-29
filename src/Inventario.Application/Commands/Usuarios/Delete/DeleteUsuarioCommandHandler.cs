using Inventario.Application.Common.Models;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
namespace Inventario.Application.Commands.Usuarios.Delete
{
    internal sealed class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand, Result>
    {
        private readonly IUsuarioRepository _repository;
        private readonly IUnitOfWork _unitOfWork;
        public DeleteUsuarioCommandHandler(IUsuarioRepository repository, IUnitOfWork unitOfWork)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }
        public async Task<Result> Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) return Result.Failure("Usuario no encontrado.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
            return Result.Success();
        }
    }
}
