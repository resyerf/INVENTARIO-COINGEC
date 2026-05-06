using Inventario.Domain.Enums;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class SolicitudInsumo : AggregateRoot
    {
        public Guid InsumoId { get; private set; }
        public Insumo Insumo { get; private set; } = null!;
        public int Cantidad { get; private set; }
        public Guid UsuarioId { get; private set; } // Solicitante
        public Usuario Usuario { get; private set; } = null!;
        public DateTime FechaSolicitud { get; private set; }
        public EstadoSolicitud Estado { get; private set; }
        public string? Observaciones { get; private set; }
        public string? RespuestaAdmin { get; private set; }
        public DateTime? FechaRespuesta { get; private set; }

        private SolicitudInsumo() { }

        public static SolicitudInsumo Create(Guid insumoId, int cantidad, Guid usuarioId, string? observaciones = null)
        {
            if (cantidad <= 0) throw new ArgumentException("La cantidad debe ser mayor a cero.");

            return new SolicitudInsumo
            {
                Id = Guid.NewGuid(),
                InsumoId = insumoId,
                Cantidad = cantidad,
                UsuarioId = usuarioId,
                FechaSolicitud = DateTime.UtcNow,
                Estado = EstadoSolicitud.PENDIENTE,
                Observaciones = observaciones?.ToUpperInvariant()
            };
        }

        public void Aprobar(string? respuesta = null)
        {
            if (Estado != EstadoSolicitud.PENDIENTE)
                throw new InvalidOperationException("Solo se pueden aprobar solicitudes pendientes.");

            Estado = EstadoSolicitud.APROBADO;
            RespuestaAdmin = respuesta?.ToUpperInvariant();
            FechaRespuesta = DateTime.UtcNow;
        }

        public void Rechazar(string? respuesta = null)
        {
            if (Estado != EstadoSolicitud.PENDIENTE)
                throw new InvalidOperationException("Solo se pueden rechazar solicitudes pendientes.");

            Estado = EstadoSolicitud.RECHAZADO;
            RespuestaAdmin = respuesta?.ToUpperInvariant();
            FechaRespuesta = DateTime.UtcNow;
        }
    }
}
