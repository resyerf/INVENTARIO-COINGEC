using Inventario.Domain.Interfaces.Repositories;
using MediatR;
namespace Inventario.Application.Commands.Usuarios.Delete
{
    internal sealed class DeleteUsuarioCommandHandler : IRequestHandler<DeleteUsuarioCommand>
    {
        private readonly IUsuarioRepository _repository;
        private readonly Inventario.Domain.Primitives.IUnitOfWork _unitOfWork;
        public DeleteUsuarioCommandHandler(IUsuarioRepository repository, Inventario.Domain.Primitives.IUnitOfWork unitOfWork)
        {
            _repository = repository;
            _unitOfWork = unitOfWork;
        }
        public async Task Handle(DeleteUsuarioCommand request, CancellationToken cancellationToken)
        {
            var entity = await _repository.GetByIdAsync(request.Id, cancellationToken);
            if (entity == null) throw new Exception("Usuario no encontrado.");
            entity.Deactivate();
            await _unitOfWork.SaveChangesAsync(cancellationToken);
        }
    }
}
