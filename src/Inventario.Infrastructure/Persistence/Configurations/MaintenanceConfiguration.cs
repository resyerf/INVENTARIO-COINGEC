using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class MaintenanceConfiguration : IEntityTypeConfiguration<Mantenimiento>
{
    public void Configure(EntityTypeBuilder<Mantenimiento> builder)
    {
        builder.ToTable("asset_maintenances");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("maintenance_id");

        // FechaMantenimiento es opcional en la DB
        builder.Property(x => x.FechaMantenimiento)
            .HasColumnName("maintenance_date")
            .IsRequired(false);

        // FechaCalibracion es opcional en la DB
        builder.Property(x => x.FechaCalibracion)
            .HasColumnName("calibration_date")
            .IsRequired(false);

        builder.Property(x => x.Tipo)
            .HasColumnName("maintenance_type")
            .HasConversion<string>() // Lo guarda como texto "Calibracion" en la DB para que sea legible
            .HasMaxLength(50)
            .IsRequired();

        builder.Property(x => x.Resultado)
            .HasColumnName("result")
            .HasMaxLength(500);

        // Relación limpia para evitar ActivoId1
        builder.HasOne(m => m.Activo)
            .WithMany(a => a.Mantenimientos)
            .HasForeignKey(m => m.ActivoId)
            .IsRequired();

        // Mapeo explícito de la columna para evitar duplicados
        builder.Property(x => x.ActivoId).HasColumnName("asset_id");

        builder.Property(x => x.Costo)
            .HasColumnName("cost")
            .HasPrecision(18, 2);
    }
}