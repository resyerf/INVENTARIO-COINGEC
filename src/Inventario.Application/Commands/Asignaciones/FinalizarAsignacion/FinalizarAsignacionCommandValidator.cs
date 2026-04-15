using FluentValidation;

namespace Inventario.Application.Commands.Asignaciones.FinalizarAsignacion
{
    public sealed class FinalizarAsignacionCommandValidator : AbstractValidator<FinalizarAsignacionCommand>
    {
        public FinalizarAsignacionCommandValidator()
        {
            RuleFor(x => x.AsignacionId)
                .NotEmpty().WithMessage("El ID de asignación es obligatorio.");

            RuleFor(x => x.EstadoRecibido)
                .NotEmpty().WithMessage("Debes registrar el estado en el que se recibe el equipo.");

            RuleFor(x => x.Observaciones)
                .MaximumLength(500).WithMessage("Las observaciones no pueden exceder los 500 caracteres.");
        }
    }
}
