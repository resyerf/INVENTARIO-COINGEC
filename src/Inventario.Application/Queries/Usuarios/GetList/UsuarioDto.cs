namespace Inventario.Application.Queries.Usuarios.GetList
{
    public record UsuarioDto(
    Guid Id,
    string NombreCompleto,
    string Area,
    string Cargo);
}
