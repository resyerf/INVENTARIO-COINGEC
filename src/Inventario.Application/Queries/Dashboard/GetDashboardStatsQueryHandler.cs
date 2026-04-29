using Inventario.Application.Common.Models;
using Inventario.Application.DTOs;
using Inventario.Domain.Interfaces.Repositories;
using MediatR;

namespace Inventario.Application.Queries.Dashboard
{
    internal sealed class GetDashboardStatsQueryHandler : IRequestHandler<GetDashboardStatsQuery, Result<DashboardStatsDto>>
    {
        private readonly IActivoRepository _activoRepository;
        private readonly IAsignacionRepository _asignacionRepository;
        private readonly ICategoriaRepository _categoriaRepository;
        private readonly ISubCategoryRepository _subCategoryRepository;
        private readonly IUbicacionRepository _ubicacionRepository;
        private readonly IUsuarioRepository _usuarioRepository;

        public GetDashboardStatsQueryHandler(
            IActivoRepository activoRepository,
            IAsignacionRepository asignacionRepository,
            ICategoriaRepository categoriaRepository,
            ISubCategoryRepository subCategoryRepository,
            IUbicacionRepository ubicacionRepository,
            IUsuarioRepository usuarioRepository)
        {
            _activoRepository = activoRepository ?? throw new ArgumentNullException(nameof(activoRepository));
            _asignacionRepository = asignacionRepository ?? throw new ArgumentNullException(nameof(asignacionRepository));
            _categoriaRepository = categoriaRepository ?? throw new ArgumentNullException(nameof(categoriaRepository));
            _subCategoryRepository = subCategoryRepository ?? throw new ArgumentNullException(nameof(subCategoryRepository));
            _ubicacionRepository = ubicacionRepository ?? throw new ArgumentNullException(nameof(ubicacionRepository));
            _usuarioRepository = usuarioRepository ?? throw new ArgumentNullException(nameof(usuarioRepository));
        }

        public async Task<Result<DashboardStatsDto>> Handle(GetDashboardStatsQuery request, CancellationToken cancellationToken)
        {
            // Obtener conteos de manera secuencial usando COUNT en la BD (mucho más eficiente)
            var totalUsuarios = await _usuarioRepository.CountAsync(cancellationToken);
            var totalActivos = await _activoRepository.CountAsync(cancellationToken);
            var activosAsignados = await _asignacionRepository.CountActivosAsignadosAsync(cancellationToken);
            var totalUbicaciones = await _ubicacionRepository.CountAsync(cancellationToken);
            var totalCategorias = await _categoriaRepository.CountAsync(cancellationToken);
            var totalSubcategorias = await _subCategoryRepository.CountAsync(cancellationToken);

            var activosNoAsignados = totalActivos - activosAsignados;

            return Result<DashboardStatsDto>.Success(new DashboardStatsDto(
                TotalUsuarios: totalUsuarios,
                TotalActivos: totalActivos,
                ActivosAsignados: activosAsignados,
                ActivosNoAsignados: activosNoAsignados,
                TotalUbicaciones: totalUbicaciones,
                TotalCategorias: totalCategorias,
                TotalSubcategorias: totalSubcategorias
            ));
        }
    }
}
