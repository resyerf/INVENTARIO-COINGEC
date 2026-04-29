using Inventario.Application.Common.Models;
using MediatR;

namespace Inventario.Application.Commands.SubCategorias.CreateMasiv
{
    public record class CreateMasivSubCategoriaCommand(string nombres, Guid categoriaId) : IRequest<Result>;
}
