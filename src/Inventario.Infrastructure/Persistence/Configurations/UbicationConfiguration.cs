using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    internal class UbicationConfiguration : IEntityTypeConfiguration<Ubicacion>
    {
        public void Configure(EntityTypeBuilder<Ubicacion> builder)
        {
            builder.ToTable("locations");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("location_id");
            builder.Property(x => x.Nombre).HasColumnName("name").HasMaxLength(100).IsRequired();

            builder.HasIndex(x => x.Nombre).IsUnique();
        }
    }
}
