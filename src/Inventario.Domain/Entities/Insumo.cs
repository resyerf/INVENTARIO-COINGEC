using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Insumo : AggregateRoot
    {
        public string Nombre { get; private set; } = string.Empty;
        public int StockActual { get; private set; }
        public string UnidadMedida { get; private set; } = string.Empty;
        public string? Descripcion { get; private set; }
        public bool IsActive { get; private set; } = true;
        public Guid CategoriaId { get; private set; }
        public Categoria Categoria { get; private set; } = null!;

        private Insumo() { }

        public static Insumo Create(string nombre, string unidadMedida, Guid categoriaId, string? descripcion = null)
        {
            return new Insumo
            {
                Id = Guid.NewGuid(),
                Nombre = nombre.ToUpperInvariant(),
                UnidadMedida = unidadMedida.ToUpperInvariant(),
                CategoriaId = categoriaId,
                Descripcion = descripcion?.ToUpperInvariant(),
                StockActual = 0,
                IsActive = true
            };
        }

        public void Update(string nombre, string unidadMedida, Guid categoriaId, string? descripcion = null)
        {
            Nombre = nombre.ToUpperInvariant();
            UnidadMedida = unidadMedida.ToUpperInvariant();
            CategoriaId = categoriaId;
            Descripcion = descripcion?.ToUpperInvariant();
        }

        public void IncrementarStock(int cantidad)
        {
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            StockActual += cantidad;
        }

        public void DecrementarStock(int cantidad)
        {
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            if (StockActual < cantidad) throw new InvalidOperationException("Stock insuficiente.");
            StockActual -= cantidad;
        }

        public void Deactivate() => IsActive = false;
        public void Activate() => IsActive = true;
    }
}
