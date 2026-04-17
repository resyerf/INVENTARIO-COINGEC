using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class SubCategoryConfiguration : IEntityTypeConfiguration<SubCategoria>
    {
        public void Configure(EntityTypeBuilder<SubCategoria> builder)
        {
            builder.ToTable("sub_categories");
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Nombre)
                .HasColumnName("name")
                .HasMaxLength(200)
                .IsRequired();

            builder.Property(x => x.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            builder.HasOne(x => x.Categoria)
                .WithMany() // Una categoría tiene muchos subtipos
                .HasForeignKey(x => x.CategoriaId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
