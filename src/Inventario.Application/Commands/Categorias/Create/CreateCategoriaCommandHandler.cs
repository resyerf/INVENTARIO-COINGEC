using Inventario.Domain.Entities;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;
using Inventario.Application.Common.Models;

namespace Inventario.Application.Commands.Categorias.Create;

internal sealed class CreateCategoriaCommandHandler : IRequestHandler<CreateCategoriaCommand, Result<Guid>>
{
    private readonly ICategoriaRepository _categoriaRepository;
    private readonly IUbicacionRepository _ubicacionRepository; // Necesitamos validar la ubicación
    private readonly IUnitOfWork _unitOfWork;

    public CreateCategoriaCommandHandler(
        ICategoriaRepository categoriaRepository,
        IUbicacionRepository ubicacionRepository,
        IUnitOfWork unitOfWork)
    {
        _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
        _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
    }

    public async Task<Result<Guid>> Handle(CreateCategoriaCommand request, CancellationToken cancellationToken)
    {
        // 1. Validar código
        var existe = await _categoriaRepository.GetByCodeAndUbicacionIdAsync(request.Codigo, request.UbicacionId, cancellationToken);
        if (existe is not null)
        {
            return Result<Guid>.Failure($"El código {request.Codigo} ya existe en esta ubicación");
        }

        // 2. Validar ubicación (SOTANO/TALLER)
        var ubicacion = await _ubicacionRepository.GetByIdAsync(request.UbicacionId, cancellationToken);
        if (ubicacion is null)
        {
            return Result<Guid>.Failure("Debes asignar una ubicación válida para esta categoría.");
        }

        // 3. Crear categoría (SubTipos puede ser null o string.Empty)
        // El Factory Method de la entidad debe aceptar el null: public string? SubTipos
        var categoria = Categoria.Create(
            request.Codigo,
            request.Descripcion,   // <-- Si viene vacío en el JSON, no pasa nada
            request.Valores,
            request.UbicacionId
        );

        _categoriaRepository.Add(categoria);
        await _unitOfWork.SaveChangesAsync(cancellationToken);

        return Result<Guid>.Success(categoria.Id);
    }
}