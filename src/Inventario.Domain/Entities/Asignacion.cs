using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Asignacion : AggregateRoot
    {
        // Id ya se maneja a través del constructor base si AggregateRoot lo permite, 
        // pero lo mantenemos explícito para tu arquitectura.
        public Guid ActivoId { get; private set; }
        public Activo Activo { get; private set; } = null!;

        // Usuario que recibe el equipo (El custodio: ERICK, MATIAS, etc.)
        public Guid UsuarioId { get; private set; }
        public Usuario Usuario { get; private set; } = null!;

        public DateTime FechaAsignacion { get; private set; }
        public DateTime? FechaDevolucion { get; private set; }

        public string EstadoEntrega { get; private set; } = string.Empty; // Ej: "Bien", "OK"
        public string? EstadoRecibido { get; private set; }
        public string? Observaciones { get; private set; }

        // Borrado lógico
        public bool IsActive { get; private set; } = true;

        // Constructor privado para forzar el uso de Create
        private Asignacion() { }

        private Asignacion(Guid id, Guid activoId, Guid usuarioId, string estadoEntrega) : base(id)
        {
            ActivoId = activoId;
            UsuarioId = usuarioId;
            FechaAsignacion = DateTime.UtcNow;
            EstadoEntrega = estadoEntrega?.ToUpperInvariant() ?? string.Empty;
            IsActive = true;
        }

        // Factory Method: Este es el que usarás en el Activo.RegistrarNuevaAsignacion
        public static Asignacion Create(Guid activoId, Guid usuarioId, string estadoEntrega)
        {
            // Aquí podrías agregar validaciones de dominio si fueran necesarias
            return new Asignacion(Guid.NewGuid(), activoId, usuarioId, estadoEntrega);
        }

        public void FinalizarAsignacion(string estadoRecibido, string? observaciones)
        {
            FechaDevolucion = DateTime.UtcNow;
            EstadoRecibido = estadoRecibido?.ToUpperInvariant();
            Observaciones = observaciones?.ToUpperInvariant();
        }

        /// <summary>
        /// Desactiva la asignación de forma lógica (borrado lógico)
        /// </summary>
        public void Deactivate()
        {
            IsActive = false;
        }

        /// <summary>
        /// Reactiva la asignación si fue desactivada
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}