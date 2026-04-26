using MediatR;

namespace Inventario.Application.Commands.Usuarios.Update
{
    public record UpdateUsuarioCommand(
    Guid Id,
    string NombreCompleto,
    string DocumentoIdentidad,
    string Email,
    string Area, // TI, Contabilidad, Operaciones
    string Cargo,
    string Sede) : IRequest<Guid>;
}
