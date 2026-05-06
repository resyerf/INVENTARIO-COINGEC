using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class CompraInsumo : AggregateRoot
    {
        public Guid InsumoId { get; private set; }
        public Insumo Insumo { get; private set; } = null!;
        public int Cantidad { get; private set; }
        public decimal PrecioUnitario { get; private set; }
        public DateTime FechaCompra { get; private set; }
        public Guid UsuarioId { get; private set; } // Admin que registra
        public Usuario Usuario { get; private set; } = null!;
        public string? Observaciones { get; private set; }

        private CompraInsumo() { }

        public static CompraInsumo Create(Guid insumoId, int cantidad, decimal precioUnitario, Guid usuarioId, string? observaciones = null)
        {
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");
            
            return new CompraInsumo
            {
                Id = Guid.NewGuid(),
                InsumoId = insumoId,
                Cantidad = cantidad,
                PrecioUnitario = precioUnitario,
                FechaCompra = DateTime.UtcNow,
                UsuarioId = usuarioId,
                Observaciones = observaciones?.ToUpperInvariant()
            };
        }
    }
}
