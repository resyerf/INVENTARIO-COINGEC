using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Categorias.Create;

internal sealed class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, Guid>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUbicacionRepository _ubicacionRepository; // Necesitamos validar la ubicación
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoriaCommandHandler(
        ICategoriaRepository categoriaRepository,
        IUbicacionRepository ubicacionRepository,
        IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository;
        _ubicacionRepository = ubicacionRepository;
        _unitOfWork = unitOfWork;
    }

    public async Task<Guid> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar código
        var existe = await _categoriaRepository.GetByCodeAsync(request.Codigo, cancellationToken);
        if (existe is not null)
        {
            throw new Exception($"El código {request.Codigo} ya existe.");
        }

        // 2. Validar ubicación (SOTANO/TALLER)
        var ubicacion = await _ubicacionRepository.GetByIdAsync(request.UbicacionId, cancellationToken);
        if (ubicacion is null)
        {
            throw new Exception("Debes asignar una ubicación válida para esta categoría.");
        }

        // 3. Crear categoría (SubTipos puede ser null o string.Empty)
        // El Factory Method de la entidad debe aceptar el null: public string? SubTipos
        var categoria = Categoria.Create(
            request.Codigo,
            request.Descripcion,   // <-- Si viene vacío en el JSON, no pasa nada
            request.UbicacionId
        );

        _categoriaRepository.Add(categoria);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return categoria.Id;
    }
}