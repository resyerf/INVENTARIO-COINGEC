using FluentValidation;
using Inventario.Application.Commands.Usuarios.Create;
using Inventario.Domain.Interfaces.Repositories;

namespace Inventario.Application.Usuarios.Create;

public class CreateUsuarioCommandValidator : AbstractValidator<CreateUsuarioCommand>
{
    private readonly IUsuarioRepository _usuarioRepository;

    public CreateUsuarioCommandValidator(IUsuarioRepository usuarioRepository)
    {
        _usuarioRepository = usuarioRepository;

        // --- VALIDACIONES DE FORMATO Y LONGITUD (Sincronizadas con Configuration) ---

        RuleFor(c => c.NombreCompleto)
            .NotEmpty().WithMessage("El nombre completo es obligatorio.")
            .MaximumLength(200).WithMessage("El nombre no puede exceder los 200 caracteres.");

        RuleFor(c => c.Area)
            .NotEmpty().WithMessage("El área o departamento es obligatorio.")
            .MaximumLength(100).WithMessage("El nombre del área no puede exceder los 100 caracteres.");

        RuleFor(c => c.Sede)
            .NotEmpty().WithMessage("La sede es obligatoria.")
            .MaximumLength(100).WithMessage("La ubicación de la sede no puede exceder los 100 caracteres.");

        RuleFor(c => c.CreadoPor)
            .NotEmpty().WithMessage("El usuario que registra es obligatorio.");

        // --- VALIDACIONES CONTRA BASE DE DATOS (Async) ---

        // Validación de Email Único
        RuleFor(c => c.Email)
            .NotEmpty().WithMessage("El correo electrónico es obligatorio.")
            .EmailAddress().WithMessage("El formato del correo electrónico no es válido.")
            .MaximumLength(150).WithMessage("El email no puede exceder los 150 caracteres.")
            .MustAsync(BeUniqueEmail).WithMessage("Este correo electrónico ya está registrado.");

        // Validación de Documento Único
        RuleFor(c => c.DocumentoIdentidad)
            .NotEmpty().WithMessage("El documento de identidad es requerido.")
            .MaximumLength(20).WithMessage("El documento no puede exceder los 20 caracteres.")
            .MustAsync(BeUniqueDocument).WithMessage("Este documento de identidad ya está registrado.");
    }

    // Método para verificar si el email ya existe
    private async Task<bool> BeUniqueEmail(string email, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(email, cancellationToken);
        return usuario == null;
    }

    // Método para verificar si el documento ya existe
    private async Task<bool> BeUniqueDocument(string documento, CancellationToken cancellationToken)
    {
        var usuario = await _usuarioRepository.GetByDocumentNbrAsync(documento, cancellationToken);
        return usuario == null;
    }
}