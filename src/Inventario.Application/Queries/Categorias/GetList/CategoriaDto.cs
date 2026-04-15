namespace Inventario.Application.Queries.Categorias.GetList
{
    public record CategoriaDto(
        Guid Id,
        string Codigo,
        string Descripcion,
        string Ubicacion);
}
