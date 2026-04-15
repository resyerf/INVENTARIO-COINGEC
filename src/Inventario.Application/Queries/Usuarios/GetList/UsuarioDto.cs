namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record UsuarioDto(
    Guid Id,
    string NombreCompleto,
    string Email,
    string Area,
    string Cargo);
}
