using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class SolicitudInsumoConfiguration : IEntityTypeConfiguration<SolicitudInsumo>
    {
        public void Configure(EntityTypeBuilder<SolicitudInsumo> builder)
        {
            builder.ToTable("supply_requests");
            builder.HasKey(x => x.Id);
            builder.Property(x => x.Id).HasColumnName("request_id");

            builder.Property(x => x.InsumoId).HasColumnName("supply_id");
            builder.Property(x => x.UsuarioId).HasColumnName("user_id");

            builder.Property(x => x.Cantidad)
                .HasColumnName("quantity")
                .IsRequired();

            builder.Property(x => x.FechaSolicitud)
                .HasColumnName("request_date")
                .IsRequired();

            builder.Property(x => x.Estado)
                .HasColumnName("status")
                .HasConversion<string>()
                .HasMaxLength(50)
                .IsRequired();

            builder.Property(x => x.Observaciones)
                .HasColumnName("remarks")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.RespuestaAdmin)
                .HasColumnName("admin_response")
                .HasMaxLength(1000)
                .IsRequired(false);

            builder.Property(x => x.FechaRespuesta)
                .HasColumnName("response_date")
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
