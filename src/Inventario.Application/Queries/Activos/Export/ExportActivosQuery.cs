using MediatR;

namespace Inventario.Application.Queries.Activos.Export
{
    public record ExportActivosQuery() : IRequest<byte[]>;
}
