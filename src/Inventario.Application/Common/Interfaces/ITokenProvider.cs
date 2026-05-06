using Inventario.Domain.Entities;

namespace Inventario.Application.Common.Interfaces
{
    public interface ITokenProvider
    {
        string Generate(AuthUser user);
    }
}
