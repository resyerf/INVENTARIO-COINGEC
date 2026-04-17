using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class SubCategoria : AggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty; // CINTA METRICA 100M, COMBA 20LB

        // Relación con su Padre (La Categoría AGEOF)
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = null!;

        // Borrado lógico
        public bool IsActive { get; private set; } = true;

        private SubCategoria() { }

        public static SubCategoria Create(string nombre, Guid categoriaId)
        {
            return new SubCategoria
            {
                Id = Guid.NewGuid(),
                Nombre = nombre,
                CategoriaId = categoriaId,
                IsActive = true
            };
        }

        /// <summary>
        /// Desactiva la subcategoría de forma lógica (borrado lógico)
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactiva la subcategoría si fue desactivada
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}
