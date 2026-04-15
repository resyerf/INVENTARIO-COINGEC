using Inventario.Domain.Enums;
using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{

    public sealed class Mantenimiento : AggregateRoot
    {
        public DateTime? FechaMantenimiento { get; private set; }
        public DateTime? FechaCalibracion { get; private set; }
        public MaintenanceType Tipo { get; private set; }
        public string? Resultado { get; private set; }
        public decimal? Costo { get; private set; }

        public Guid ActivoId { get; private set; }
        public Activo Activo { get; private set; } = null!;

        // Constructor para EF Core
        private Mantenimiento() { }

        // Factory Method: Centraliza la creación y validación
        public static Mantenimiento Create(
            Guid activoId,
            MaintenanceType tipo,
            DateTime? fechaMantenimiento = null,
            DateTime? fechaCalibracion = null,
            string? resultado = null,
            decimal? costo = null)
        {
            // Regla: No se puede crear un mantenimiento sin al menos una intención de fecha o tipo
            return new Mantenimiento(
                Guid.NewGuid(),
                activoId,
                tipo,
                fechaMantenimiento,
                fechaCalibracion,
                resultado,
                costo);
        }

        private Mantenimiento(
            Guid id,
            Guid activoId,
            MaintenanceType tipo,
            DateTime? fechaMantenimiento,
            DateTime? fechaCalibracion,
            string? resultado,
            decimal? costo) : base(id)
        {
            ActivoId = activoId;
            Tipo = tipo;
            FechaMantenimiento = fechaMantenimiento;
            FechaCalibracion = fechaCalibracion;
            Resultado = resultado;
            Costo = costo;
        }
    }
}