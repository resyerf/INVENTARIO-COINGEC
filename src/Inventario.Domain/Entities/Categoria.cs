using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Categoria : AggregateRoot
    {
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;
        public string Valores { get; private set; } = string.Empty;

        // Solo añadimos esto para que sepa si es de SOTANO o TALLER
        public Guid UbicacionId { get; private set; }
        public Ubicacion Ubicacion { get; private set; } = null!;

        // Borrado lógico
        public bool IsActive { get; private set; } = true;

        private Categoria() { }

        private Categoria(Guid id, string codigo, string descripcion, string valores, Guid ubicacionId)
            : base(id)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            Valores = string.IsNullOrWhiteSpace(valores) ? string.Empty : valores.Trim().ToUpperInvariant();
            UbicacionId = ubicacionId;
            IsActive = true;
        }

        public static Categoria Create(string codigo, string descripcion, string valores, Guid ubicacionId)
        {
            return new Categoria(
                Guid.NewGuid(), 
                codigo?.Trim().ToUpperInvariant() ?? string.Empty, 
                descripcion?.ToUpperInvariant() ?? string.Empty, 
                valores?.ToUpperInvariant() ?? string.Empty, 
                ubicacionId
            );
        }

        public void Update(string codigo, string descripcion, string valores, Guid ubicacionId)
        {
            Codigo = codigo?.Trim().ToUpperInvariant() ?? string.Empty;
            Descripcion = descripcion?.ToUpperInvariant() ?? string.Empty;
            Valores = string.IsNullOrWhiteSpace(valores) ? string.Empty : valores.Trim().ToUpperInvariant();
            UbicacionId = ubicacionId;
        }

        /// <summary>
        /// Desactiva la categoría de forma lógica (borrado lógico)
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactiva la categoría si fue desactivada
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}