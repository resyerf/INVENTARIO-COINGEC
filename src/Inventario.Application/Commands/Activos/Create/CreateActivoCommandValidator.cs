using FluentValidation;
using Inventario.Application.Commands.Activos.Create;
using Inventario.Domain.Interfaces.Repositories;

namespace Inventario.Application.Activos.Validators
{
    public sealed class CreateActivoCommandValidator : AbstractValidator<CreateActivoCommand>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly ISubCategoryRepository _subCategoriaRepository;
        private readonly IUbicacionRepository _ubicacionRepository;

        public CreateActivoCommandValidator(
            IActivoRepository activoRepository,
            ISubCategoryRepository subCategoriaRepository,
            IUbicacionRepository ubicacionRepository)
        {
            _activoRepository = activoRepository;
            _subCategoriaRepository = subCategoriaRepository;
            _ubicacionRepository = ubicacionRepository;

            // 1. Nombre del equipo obligatorio
            RuleFor(x => x.NombreEquipo)
                .NotEmpty().WithMessage("El nombre del equipo es obligatorio.")
                .MaximumLength(250).WithMessage("El nombre no puede exceder los 250 caracteres.");

            // 2. Cantidad no menor que 0
            RuleFor(x => x.Cantidad)
                .GreaterThanOrEqualTo(0).WithMessage("La cantidad no puede ser menor a 0.");

            // 3. Subcategoría debe existir
            RuleFor(x => x.SubCategoriaId)
                .NotEmpty().WithMessage("La subcategoría es obligatoria.")
                .MustAsync(async (id, cancellation) =>
                {
                    var existe = await _subCategoriaRepository.GetByIdAsync(id, cancellation);
                    return existe != null; // Retorna true si existe, false si es null
                })
                .WithMessage("La subcategoría especificada no existe.");

            // 4. Ubicación debe existir (si se proporciona)
            RuleFor(x => x.UbicacionId)
                .MustAsync(async (id, cancellation) =>
                {
                    if (id == null || id == Guid.Empty) return true;

                    var existe = await _ubicacionRepository.GetByIdAsync(id.Value, cancellation);
                    return existe != null;
                })
                .WithMessage("La ubicación especificada no existe.");

            // 5. Serial Único (Solo si se proporciona un serial)
            RuleFor(x => x.Serie)
                .MustAsync(async (serie, cancellation) =>
                {
                    if (string.IsNullOrWhiteSpace(serie)) return true;

                    // Usamos el método que ya tienes en ActivoRepository
                    var existe = await _activoRepository.GetBySerialNumberAsync(serie, cancellation);
                    return existe == null; // Es válido solo si NO existe
                })
                .WithMessage(x => $"El número de serie '{x.Serie}' ya está registrado.");

            // El CostoUnitario no tiene regla de 'NotEmpty', así que es opcional por defecto.
        }
    }
}