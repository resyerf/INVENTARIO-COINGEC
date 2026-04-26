using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Usuarios.Update
{
    internal sealed class UpdateUsuarioCommandHandler : IRequestHandler<UpdateUsuarioCommand, Guid>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public UpdateUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(UpdateUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = await _usuarioRepository.GetByIdAsync(request.Id);

            if (usuario == null)
            {
                throw new KeyNotFoundException($"No se encontró el usuario con ID {request.Id}");
            }

            usuario.Update(
                request.NombreCompleto,
                request.DocumentoIdentidad,
                request.Email,
                request.Area,
                request.Cargo,
                request.Sede
            );

            _usuarioRepository.Update(usuario);
            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return usuario.Id;
        }
    }
}
