using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class MovimientoInsumoConfiguration : IEntityTypeConfiguration<MovimientoInsumo>
    {
        public void Configure(EntityTypeBuilder<MovimientoInsumo> builder)
        {
            builder.ToTable("supply_movements");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("movement_id");

            builder.Property(x => x.InsumoId).HasColumnName("supply_id");

            builder.Property(x => x.Cantidad)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(x => x.Tipo)
                .HasColumnName("type")
                .HasConversion<string>()
                .HasMaxLength(20)
                .IsRequired();

            builder.Property(x => x.Motivo)
                .HasColumnName("reason")
                .HasMaxLength(100)
                .IsRequired();

            builder.Property(x => x.Fecha)
                .HasColumnName("movement_date")
                .IsRequired();

            builder.Property(x => x.ReferenciaId)
                .HasColumnName("reference_id")
                .IsRequired(false);

            builder.HasOne(x => x.Insumo)
                .WithMany()
                .HasForeignKey(x => x.InsumoId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
