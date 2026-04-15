using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Categoria : AggregateRoot
    {
        public string Codigo { get; private set; } = string.Empty;
        public string Descripcion { get; private set; } = string.Empty;

        // Solo añadimos esto para que sepa si es de SOTANO o TALLER
        public Guid UbicacionId { get; private set; }
        public Ubicacion Ubicacion { get; private set; } = null!;

        private Categoria() { }

        private Categoria(Guid id, string codigo, string descripcion, Guid ubicacionId)
            : base(id)
        {
            Codigo = codigo;
            Descripcion = descripcion;
            UbicacionId = ubicacionId;
        }

        public static Categoria Create(string codigo, string descripcion, Guid ubicacionId)
        {
            return new Categoria(Guid.NewGuid(), codigo.Trim().ToUpper(), descripcion, ubicacionId);
        }
    }
}