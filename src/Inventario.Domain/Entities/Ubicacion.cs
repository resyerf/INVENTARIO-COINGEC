using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Ubicacion : AggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }

        // Borrado lógico
        public bool IsActive { get; private set; } = true;

        private Ubicacion() { }

        public static Ubicacion Create(string nombre, string? descripcion = null)
        {
            return new Ubicacion
            {
                Id = Guid.NewGuid(),
                Nombre = nombre.ToUpper().Trim(),
                Descripcion = descripcion,
                IsActive = true
            };
        }

        /// <summary>
        /// Desactiva la ubicación de forma lógica (borrado lógico)
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactiva la ubicación si fue desactivada
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}