using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class InsumoConfiguration : IEntityTypeConfiguration<Insumo>
    {
        public void Configure(EntityTypeBuilder<Insumo> builder)
        {
            builder.ToTable("supplies");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("supply_id");

            builder.Property(x => x.Nombre)
                .HasColumnName("name")
                .HasMaxLength(250)
                .IsRequired();

            builder.Property(x => x.StockActual)
                .HasColumnName("current_stock")
                .HasDefaultValue(0);

            builder.Property(x => x.UnidadMedida)
                .HasColumnName("unit_of_measure")
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Descripcion)
                .HasColumnName("description")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.Property(x => x.CategoriaId)
                .HasColumnName("category_id");

            builder.HasOne(x => x.Categoria)
                .WithMany()
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
