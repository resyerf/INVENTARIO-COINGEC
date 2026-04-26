using Inventario.Application.Commands.Activos.Import;
using MediatR;

namespace Inventario.Application.Commands.Usuarios.Import
{
    public record ImportUsuariosCommand(Stream FileStream, string FileName) : IRequest<ImportResult>;
}
