using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class AssetConfiguration : IEntityTypeConfiguration<Activo>
    {
        public void Configure(EntityTypeBuilder<Activo> builder)
        {
            builder.ToTable("assets");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("asset_id");

            builder.Property(x => x.NombreEquipo)
                .HasColumnName("equipment_name")
                .HasMaxLength(250)
                .IsRequired(); // Este es el único obligatorio para saber qué es

            builder.Property(x => x.CodigoEquipo)
                .HasColumnName("equipment_code")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Marca)
                .HasColumnName("brand")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.Modelo)
                .HasColumnName("model")
                .HasMaxLength(100)
                .IsRequired(false);

            builder.Property(x => x.Serie)
                .HasColumnName("serial_number")
                .HasMaxLength(250)
                .IsRequired(false);

            builder.Property(x => x.Etiquetado)
                .HasColumnName("labeling_status")
                .HasMaxLength(50)
                .HasDefaultValue("-")
                .IsRequired(false); // "Etiquetado por defecto -"

            builder.Property(x => x.CostoUnitario)
                .HasColumnName("unit_cost")
                .HasPrecision(18, 2)
                .HasDefaultValue(0);

            builder.Property(x => x.Cantidad)
                .HasColumnName("quantity")
                .HasDefaultValue(1);

            builder.Property(x => x.Estado)
                .HasColumnName("condition_status")
                .HasMaxLength(50)
                .IsRequired(false);

            builder.Property(x => x.Observaciones)
                .HasColumnName("remarks")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.FechaAdquisicion)
                .HasColumnName("acquisition_date")
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            // Relaciones
            builder.HasOne(x => x.SubCategoria)
                .WithMany()
                .HasForeignKey(x => x.SubCategoriaId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario).WithMany().HasForeignKey(x => x.UsuarioId).IsRequired(false);
            builder.HasOne(x => x.Ubicacion).WithMany().HasForeignKey(x => x.UbicacionId).IsRequired(false);
        }
    }
}