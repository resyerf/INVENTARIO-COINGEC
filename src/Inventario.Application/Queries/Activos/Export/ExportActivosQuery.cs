using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Queries.Activos.Export
{
    public record ExportActivosQuery() : IRequest<Result<byte[]>>;
}
