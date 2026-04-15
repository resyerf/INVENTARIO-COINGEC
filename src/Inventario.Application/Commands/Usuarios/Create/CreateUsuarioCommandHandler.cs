using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Usuarios.Create
{
    internal sealed class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, Guid>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
        {
            var usuario = Usuario.Create(
                request.NombreCompleto,
                request.DocumentoIdentidad,
                request.Email,
                request.Area,
                request.Cargo,
                request.Sede
            );

            _usuarioRepository.Add(usuario);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return usuario.Id;
        }
    }
}
