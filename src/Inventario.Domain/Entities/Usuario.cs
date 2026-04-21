using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class Usuario : AggregateRoot
    {
        public string NombreCompleto { get; private set; } = string.Empty;
        public string DocumentoIdentidad { get; private set; } = string.Empty;
        public string Email { get; private set; } = string.Empty;
        public string Area { get; private set; } = string.Empty;
        public string? Cargo { get; private set; }
        public string? Sede { get; private set; }
        
        // Borrado lógico
        public bool IsActive { get; private set; } = true;

        public ICollection<Activo> ActivosAsignados { get; private set; } = new List<Activo>();

        // Constructor privado para obligar el uso de .Create()
        private Usuario() { }

        // Constructor interno usado por el Factory
        private Usuario(
            Guid id,
            string nombreCompleto,
            string documentoIdentidad,
            string email,
            string area,
            string cargo,
            string sede) : base(id)
        {
            NombreCompleto = nombreCompleto?.ToUpperInvariant() ?? string.Empty;
            DocumentoIdentidad = documentoIdentidad?.ToUpperInvariant() ?? string.Empty;
            Email = email; // Mantenemos el formato original
            Area = area?.ToUpperInvariant() ?? string.Empty;
            Cargo = cargo?.ToUpperInvariant();
            Sede = sede?.ToUpperInvariant();
            IsActive = true;
        }

        // --- STATIC FACTORY METHOD ---
        public static Usuario Create(
            string nombreCompleto,
            string documentoIdentidad,
            string email,
            string area,
            string cargo,
            string sede)
        {
            // Aquí puedes agregar validaciones de negocio rápidas
            if (string.IsNullOrWhiteSpace(email))
                throw new ArgumentException("El email no puede estar vacío.");

            var usuario = new Usuario(
                Guid.NewGuid(),
                nombreCompleto?.ToUpperInvariant(),
                documentoIdentidad,
                email,
                area?.ToUpperInvariant(),
                cargo?.ToUpperInvariant(),
                sede?.ToUpperInvariant()
            );

            // Ejemplo: Si quisieras disparar un evento cuando se crea un usuario
            // usuario.Raise(new UsuarioCreadoDomainEvent(usuario.Id));

            return usuario;
        }

        /// <summary>
        /// Desactiva el usuario de forma lógica (borrado lógico)
        /// </summary>
        public void Deactivate()
        {
            if (ActivosAsignados.Any(a => a.Estado == "Asignado"))
                throw new InvalidOperationException("No se puede desactivar un usuario con activos.");

            IsActive = false;
        }

        /// <summary>
        /// Reactiva el usuario si fue desactivado
        /// </summary>
        public void Activate()
        {
            IsActive = true;
        }
    }
}