using FluentValidation;
using Inventario.Domain.Interfaces.Repositories;

namespace Inventario.Application.Commands.Asignaciones.Asignar
{
    public sealed class AsignacionActivoCommandValidator : AbstractValidator<AsignacionActivoCommand>
    {
        public AsignacionActivoCommandValidator(IActivoRepository activoRepo, IUsuarioRepository usuarioRepo)
        {
            RuleFor(x => x.ActivoId)
                .NotEmpty()
                .MustAsync(async (id, cancel) => await activoRepo.GetByIdAsync(id) != null)
                .WithMessage("El activo no existe.");

            RuleFor(x => x.UsuarioId)
                .NotEmpty()
                .MustAsync(async (id, cancel) => await usuarioRepo.GetByIdAsync(id) != null)
                .WithMessage("El usuario (custodio) no existe.");

            RuleFor(x => x.EstadoEntrega)
                .NotEmpty().WithMessage("Debes indicar en qué estado entregas el equipo.");
            
            RuleFor(x => x.ActivoId)
                .MustAsync(async (id, cancellation) =>
                {
                    var activo = await activoRepo.GetByIdAsync(id, cancellation);
                    // Regla: Solo se puede asignar si no tiene un usuario asignado actualmente
                    return activo != null && activo.UsuarioId == null;
                })
                .WithMessage("El equipo ya se encuentra asignado a otra persona. Debe registrarse la devolución primero.");
        }
    }
}
