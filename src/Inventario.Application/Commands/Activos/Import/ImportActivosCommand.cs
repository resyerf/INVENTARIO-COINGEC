using MediatR;

namespace Inventario.Application.Commands.Activos.Import
{
    public record ImportActivosCommand(Stream FileStream, string FileName) : IRequest<ImportResult>;
}
