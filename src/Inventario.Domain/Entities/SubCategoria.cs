using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class SubCategoria : AggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty; // CINTA METRICA 100M, COMBA 20LB

        // Relación con su Padre (La Categoría AGEOF)
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = null!;

        private SubCategoria() { }

        public static SubCategoria Create(string nombre, Guid categoriaId)
        {
            return new SubCategoria
            {
                Id = Guid.NewGuid(),
                Nombre = nombre,
                CategoriaId = categoriaId
            };
        }
    }
}
