using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Ubicacion : AggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }

        private Ubicacion() { }

        public static Ubicacion Create(string nombre, string? descripcion = null)
        {
            return new Ubicacion
            {
                Id = Guid.NewGuid(),
                Nombre = nombre.ToUpper().Trim(),
                Descripcion = descripcion
            };
        }
    }
}