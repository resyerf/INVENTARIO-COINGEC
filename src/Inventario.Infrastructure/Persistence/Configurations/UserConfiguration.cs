using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<Usuario>
    {
        public void Configure(EntityTypeBuilder<Usuario> builder)
        {
            builder.ToTable("users");

            builder.HasKey(u => u.Id);
            builder.Property(u => u.Id)
                .HasColumnName("user_id");

            builder.Property(u => u.NombreCompleto)
                .HasColumnName("full_name")
                .IsRequired()
                .HasMaxLength(200);

            builder.Property(u => u.DocumentoIdentidad)
                .HasColumnName("document_id")
                .IsRequired()
                .HasMaxLength(20);

            builder.Property(u => u.Email)
                .HasColumnName("email")
                .IsRequired()
                .HasMaxLength(150);

            builder.Property(u => u.Area)
                .HasColumnName("department_area")
                .HasMaxLength(100);

            builder.Property(u => u.Cargo)
                .HasColumnName("job_title")
                .HasMaxLength(100);

            builder.Property(u => u.Sede)
                .HasColumnName("office_location")
                .HasMaxLength(100);

            builder.Property(u => u.EstaActivo)
                .HasColumnName("is_active")
                .HasDefaultValue(true);

            // Índice único para evitar documentos duplicados
            builder.HasIndex(u => u.DocumentoIdentidad).IsUnique();
            builder.HasIndex(u => u.Email).IsUnique();

            // Configuración de la relación con Activos
            builder.HasMany(u => u.ActivosAsignados)
                .WithOne(a => a.Usuario)
                .HasForeignKey(a => a.UsuarioId)
                .OnDelete(DeleteBehavior.SetNull);
        }
    }
}