using MediatR;

namespace Inventario.Application.Commands.Usuarios.Create
{
    public record CreateUsuarioCommand(
    string NombreCompleto,
    string DocumentoIdentidad,
    string Email,
    string Area, // TI, Contabilidad, Operaciones
    string Cargo,
    string Sede,
    string CreadoPor) : IRequest<Guid>;
}
