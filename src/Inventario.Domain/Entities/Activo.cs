using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Activo : AggregateRoot
    {
        public string NombreEquipo { get; private set; } = string.Empty;
        public string? Marca { get; private set; }
        public string? Modelo { get; private set; }
        public string? Serie { get; private set; }
        public string Etiquetado { get; private set; } = "-"; // Valor por defecto

        public int Cantidad { get; private set; }
        public string? Estado { get; private set; }
        public decimal CostoUnitario { get; private set; }
        public string? Observaciones { get; private set; }

        // Fechas opcionales
        public DateTime? FechaAdquisicion { get; private set; }

        // Relaciones
        public Guid SubCategoriaId { get; private set; }
        public SubCategoria SubCategoria { get; private set; } = null!;

        public Guid? UsuarioId { get; private set; }
        public Usuario? Usuario { get; private set; }

        public Guid? UbicacionId { get; private set; }
        public Ubicacion? Ubicacion { get; private set; }

        private readonly List<Asignacion> _asignaciones = new();
        public IReadOnlyCollection<Asignacion> Asignaciones => _asignaciones.AsReadOnly();

        private readonly List<Mantenimiento> _mantenimientos = new();
        public IReadOnlyCollection<Mantenimiento> Mantenimientos => _mantenimientos.AsReadOnly();
        private Activo() { }

        public static Activo Create(
            string nombreEquipo,
            Guid subCategoriaId,
            decimal costoUnitario = 0,
            int cantidad = 1,
            string? marca = null,
            string? modelo = null,
            string? serie = null,
            string etiquetado = "-", // Por defecto "-"
            Guid? ubicacionId = null,
            DateTime? fechaAdquisicion = null)
        {
            return new Activo
            {
                Id = Guid.NewGuid(),
                NombreEquipo = nombreEquipo,
                SubCategoriaId = subCategoriaId,
                CostoUnitario = costoUnitario,
                Cantidad = cantidad,
                Marca = marca,
                Modelo = modelo,
                Serie = serie,
                Etiquetado = etiquetado,
                UbicacionId = ubicacionId,
                FechaAdquisicion = fechaAdquisicion.HasValue
                ? DateTime.SpecifyKind(fechaAdquisicion.Value, DateTimeKind.Utc)
                : null,
                Estado = "Bien"
            };
        }

        public void SetCustodio(Guid usuarioId)
        {
            this.UsuarioId = usuarioId;
            // Podrías agregar lógica aquí, como cambiar el estado a "En Uso"
        }

        public void LiberarCustodio()
        {
            this.UsuarioId = null; // El equipo ya no tiene dueño, vuelve a estar en almacén/disponible
        }
    }
}