using MediatR;

namespace Inventario.Domain.Primitives
{
    public record DomainEvent(Guid Id) : INotification;
}
