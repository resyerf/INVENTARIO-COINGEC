using Inventario.Domain.Enums;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class MovimientoInsumo : AggregateRoot
    {
        public Guid InsumoId { get; private set; }
        public Insumo Insumo { get; private set; } = null!;
        public int Cantidad { get; private set; }
        public TipoMovimiento Tipo { get; private set; }
        public string Motivo { get; private set; } = string.Empty; // COMPRA, SOLICITUD, AJUSTE
        public DateTime Fecha { get; private set; }
        public Guid? ReferenciaId { get; private set; } // Id de Compra o Solicitud

        private MovimientoInsumo() { }

        public static MovimientoInsumo Create(Guid insumoId, int cantidad, TipoMovimiento tipo, string motivo, Guid? referenciaId = null)
        {
            return new MovimientoInsumo
            {
                Id = Guid.NewGuid(),
                InsumoId = insumoId,
                Cantidad = cantidad,
                Tipo = tipo,
                Motivo = motivo.ToUpperInvariant(),
                Fecha = DateTime.UtcNow,
                ReferenciaId = referenciaId
            };
        }
    }
}
