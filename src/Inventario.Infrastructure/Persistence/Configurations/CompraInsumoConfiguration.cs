using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class CompraInsumoConfiguration : IEntityTypeConfiguration<CompraInsumo>
    {
        public void Configure(EntityTypeBuilder<CompraInsumo> builder)
        {
            builder.ToTable("supply_purchases");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("purchase_id");

            builder.Property(x => x.InsumoId).HasColumnName("supply_id");
            builder.Property(x => x.UsuarioId).HasColumnName("user_id");

            builder.Property(x => x.Cantidad)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(x => x.PrecioUnitario)
                .HasColumnName("unit_price")
                .HasPrecision(18, 2)
                .IsRequired();

            builder.Property(x => x.FechaCompra)
                .HasColumnName("purchase_date")
                .IsRequired();

            builder.Property(x => x.Observaciones)
                .HasColumnName("remarks")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.HasOne(x => x.Insumo)
                .WithMany()
                .HasForeignKey(x => x.InsumoId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(x => x.Usuario)
                .WithMany()
                .HasForeignKey(x => x.UsuarioId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
