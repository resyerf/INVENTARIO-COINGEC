using Inventario.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Inventario.Infrastructure.Persistence.Configurations;

public class AssignmentConfiguration : IEntityTypeConfiguration<Asignacion>
{
    public void Configure(EntityTypeBuilder<Asignacion> builder)
    {
        builder.ToTable("asset_assignments");

        builder.HasKey(x => x.Id);
        builder.Property(x => x.Id).HasColumnName("assignment_id");

        builder.Property(x => x.FechaAsignacion)
            .HasColumnName("assigned_at")
            .IsRequired();

        builder.Property(x => x.FechaDevolucion)
            .HasColumnName("returned_at");

        builder.Property(x => x.EstadoEntrega)
            .HasColumnName("delivery_status")
            .HasMaxLength(100);

        builder.Property(x => x.EstadoRecibido)
            .HasColumnName("return_status")
            .HasMaxLength(100);

        builder.Property(x => x.Observaciones)
            .HasColumnName("remarks")
            .HasMaxLength(500);

        builder.Property(x => x.IsActive)
            .HasColumnName("is_active")
            .HasDefaultValue(true);

        // --- RELACIONES CORREGIDAS ---

        // Relación con Activo
        builder.HasOne(x => x.Activo)
            .WithMany(a => a.Asignaciones) // Asegúrate de que Activo.cs tenga: public ICollection<Asignacion> Asignaciones { get; set; }
            .HasForeignKey(x => x.ActivoId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade); // Si se borra el activo (raro), se borra el historial

        // Relación con Usuario
        builder.HasOne(x => x.Usuario)
            .WithMany() // O .WithMany(u => u.Asignaciones) si lo agregaste en Usuario.cs
            .HasForeignKey(x => x.UsuarioId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict); // No permitas borrar un usuario si tiene activos asignados

        // Mapeo explícito de las FK para evitar duplicados
        builder.Property(x => x.ActivoId).HasColumnName("asset_id");
        builder.Property(x => x.UsuarioId).HasColumnName("user_id");
    }
}