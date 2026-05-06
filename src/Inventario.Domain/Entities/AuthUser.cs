using Inventario.Domain.Primitives;

namespace Inventario.Domain.Entities
{
    public sealed class AuthUser : AggregateRoot
    {
        public string Username { get; private set; } = string.Empty;
        public string PasswordHash { get; private set; } = string.Empty;
        public DateTime CreatedAt { get; private set; }
        public bool IsActive { get; private set; } = true;
        public string Role { get; private set; } = "User";

        private AuthUser() { }

        private AuthUser(Guid id, string username, string passwordHash, string role) : base(id)
        {
            Username = username.ToLowerInvariant();
            PasswordHash = passwordHash;
            CreatedAt = DateTime.UtcNow;
            Role = role;
            IsActive = true;
        }

        public static AuthUser Create(string username, string passwordHash, string role = "User")
        {
            return new AuthUser(Guid.NewGuid(), username, passwordHash, role);
        }

        public void UpdatePassword(string newPasswordHash)
        {
            PasswordHash = newPasswordHash;
        }
    }
}
