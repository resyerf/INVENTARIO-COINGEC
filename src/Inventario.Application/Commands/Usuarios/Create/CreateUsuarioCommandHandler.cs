using Inventario.Application.Common.Models;
using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Usuarios.Create
{
    internal sealed class CreateUsuarioCommandHandler : IRequestHandler<CreateUsuarioCommand, Result<Guid>>
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUnitOfWork _unitOfWork;

        public CreateUsuarioCommandHandler(
            IUsuarioRepository usuarioRepository,
            IUnitOfWork unitOfWork)
        {
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
            _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        }

        public async Task<Result<Guid>> Handle(CreateUsuarioCommand request, CancellationToken cancellationToken)
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

            return Result<Guid>.Success(usuario.Id);
        }
    }
}
