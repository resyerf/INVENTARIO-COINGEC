namespace Inventario.Application.DTOs
{
    /// <summary>
    /// DTO que contiene las estadísticas del dashboard
    /// </summary>
    public sealed record DashboardStatsDto(
        int TotalUsuarios,
        int TotalActivos,
        int ActivosAsignados,
        int ActivosNoAsignados,
        int TotalUbicaciones,
        int TotalCategorias,
        int TotalSubcategorias
    );
}
