using Inventario.Application.Interfaces.Services;
using Inventario.Domain.Entities;
using Inventario.Domain.Enums;
using Inventario.Domain.Interfaces.Repositories;
using Inventario.Domain.Primitives;
using MediatR;

namespace Inventario.Application.Commands.Compras.Registrar
{
    public record RegistrarCompraCommand(
        Guid InsumoId,
        int Cantidad,
        decimal PrecioUnitario,
        Guid UsuarioId,
        string? Observaciones
    ) : IRequest<Guid>;

    public class RegistrarCompraCommandHandler : IRequestHandler<RegistrarCompraCommand, Guid>
    {
        private readonly IInsumoRepository _insumoRepository;
        private readonly ICompraInsumoRepository _compraRepository;
        private readonly IMovimientoInsumoRepository _movimientoRepository;
        private readonly IUnitOfWork _unitOfWork;

        public RegistrarCompraCommandHandler(
            IInsumoRepository insumoRepository,
            ICompraInsumoRepository compraRepository,
            IMovimientoInsumoRepository movimientoRepository,
            IUnitOfWork unitOfWork)
        {
            _insumoRepository = insumoRepository;
            _compraRepository = compraRepository;
            _movimientoRepository = movimientoRepository;
            _unitOfWork = unitOfWork;
        }

        public async Task<Guid> Handle(RegistrarCompraCommand request, CancellationToken cancellationToken)
        {
            var insumo = await _insumoRepository.GetByIdAsync(request.InsumoId, cancellationToken);
            if (insumo == null) throw new Exception("Insumo no encontrado");

            // 1. Crear registro de compra
            var compra = CompraInsumo.Create(
                request.InsumoId,
                request.Cantidad,
                request.PrecioUnitario,
                request.UsuarioId,
                request.Observaciones
            );

            // 2. Incrementar stock
            insumo.IncrementarStock(request.Cantidad);

            // 3. Registrar movimiento
            var movimiento = MovimientoInsumo.Create(
                request.InsumoId,
                request.Cantidad,
                TipoMovimiento.ENTRADA,
                "COMPRA",
                compra.Id
            );

            _compraRepository.Add(compra);
            _movimientoRepository.Add(movimiento);
            _insumoRepository.Update(insumo);

            await _unitOfWork.SaveChangesAsync(cancellationToken);

            return compra.Id;
        }
    }
}
