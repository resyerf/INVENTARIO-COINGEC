using FluentValidation;
using Inventario.Application.Commands.Usuarios.Update;
using Inventario.Domain.Interfaces.Repositories;

namespace Inventario.Application.Usuarios.Update;

public class UpdateUsuarioCommandValidator : AbstractValidator<UpdateUsuarioCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public UpdateUsuarioCommandValidator(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;

        RuleFor(c => c.Id)
            .NotEmpty().WithMessage("El ID del usuario es obligatorio.");

        RuleFor(c => c.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder los 200 caracteres.");

        RuleFor(c => c.Area)
            .NotEmpty().WithMessage("El área o departamento es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del área no puede exceder los 100 caracteres.");

        RuleFor(c => c.Sede)
            .NotEmpty().WithMessage("La sede es obligatoria.")
            .MaximumLength(100).WithMessage("La ubicación de la sede no puede exceder los 100 caracteres.");

        // Validación de Email Único
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .MaximumLength(150).WithMessage("El email no puede exceder los 150 caracteres.")
            .MustAsync(async (command, email, cancellationToken) => await BeUniqueEmail(command.Id, email, cancellationToken))
            .WithMessage("Este correo electrónico ya está registrado en otro usuario.");

        // Validación de Documento Único
        RuleFor(c => c.DocumentoIdentidad)
            .NotEmpty().WithMessage("El documento de identidad es requerido.")
            .MaximumLength(20).WithMessage("El documento no puede exceder los 20 caracteres.")
            .MustAsync(async (command, documento, cancellationToken) => await BeUniqueDocument(command.Id, documento, cancellationToken))
            .WithMessage("Este documento de identidad ya está registrado en otro usuario.");
    }

    private async Task<bool> BeUniqueEmail(Guid id, string email, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(email, cancellationToken);
        return usuario == null || usuario.Id == id;
    }

    private async Task<bool> BeUniqueDocument(Guid id, string documento, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByDocumentNbrAsync(documento, cancellationToken);
        return usuario == null || usuario.Id == id;
    }
}
