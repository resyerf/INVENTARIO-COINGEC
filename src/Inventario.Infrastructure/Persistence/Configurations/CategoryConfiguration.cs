using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class CategoryConfiguration : IEntityTypeConfiguration<Categoria>
    {
        public void Configure(EntityTypeBuilder<Categoria> builder)
        {
            builder.ToTable("categories");
            builder.HasKey(c => c.Id);
            builder.Property(c => c.Id).HasColumnName("category_id");

            builder.Property(c => c.Codigo).HasColumnName("code").HasMaxLength(20).IsRequired();
            builder.Property(c => c.Descripcion).HasColumnName("description").HasMaxLength(200).IsRequired();

            builder.Property(c => c.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            // Relación con la tabla de Ubicaciones
            builder.HasOne(c => c.Ubicacion)
                .WithMany()
                .HasForeignKey(c => c.UbicacionId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasIndex(c => c.Codigo).IsUnique();
        }
    }
}