namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record UsuarioDto(
    Guid Id,
    string DocumentoIdentidad,
    string NombreCompleto,
    string Email,
    string Area,
    string Cargo,
    string Sede,
    bool IsActive);
}
