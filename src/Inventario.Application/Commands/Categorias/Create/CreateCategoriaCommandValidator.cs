using FluentValidation;
using Inventario.Domain.Interfaces.Repositories;

namespace Inventario.Application.Commands.Categorias.Create;

public class CreateCategoriaCommandValidator : AbstractValidator<CreateCategoriaCommand>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public CreateCategoriaCommandValidator(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;

        RuleFor(c => c.Codigo)
            .NotEmpty().WithMessage("El código de categoría es obligatorio.")
            .MaximumLength(20).WithMessage("El código no puede exceder los 20 caracteres.")
            .MustAsync(BeUniqueCode).WithMessage("Este código de categoría ya existe.");

        RuleFor(c => c.Descripcion)
            .NotEmpty().WithMessage("La descripción es obligatoria.")
            .MaximumLength(200).WithMessage("La descripción no puede exceder los 200 caracteres.");
    }

    private async Task<bool> BeUniqueCode(string codigo, CancellationToken cancellationToken)
    {
        var categoria = await _categoriaRepository.GetByCodeAsync(codigo, cancellationToken);
        return categoria is null;
    }
}