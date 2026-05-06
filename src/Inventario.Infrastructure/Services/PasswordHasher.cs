using Inventario.Application.Common.Interfaces;
using BCryptNet = BCrypt.Net.BCrypt;

namespace Inventario.Infrastructure.Services
{
    public class PasswordHasher : IPasswordHasher
    {
        public string Hash(string password) => BCryptNet.HashPassword(password);

        public bool Verify(string password, string hash) => BCryptNet.Verify(password, hash);
    }
}
